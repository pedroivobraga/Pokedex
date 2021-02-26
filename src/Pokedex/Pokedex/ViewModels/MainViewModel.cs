using MvvmHelpers;
using Pokedex.Models;
using Pokedex.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Pokedex.ViewModels
{
    public class MainViewModel : ViewModelBase, IViewModel
    {
        private readonly IPokedexService _pokedexService;
        private Paged<Pokemon> currentPaged;


        #region Properties

        private ObservableRangeCollection<Pokemon> pokemons;
        public ObservableRangeCollection<Pokemon> Pokemons
        {
            get { return pokemons; }
            set { SetProperty(ref pokemons, value); }
        }


        private ObservableRangeCollection<SelectableItem<string>> pokemonTypes;
        public ObservableRangeCollection<SelectableItem<string>> PokemonTypes
        {
            get { return pokemonTypes; }
            set { SetProperty(ref pokemonTypes, value); }
        }



        private int currentPage;
        public int CurrentPage
        {
            get { return currentPage; }
            set { SetProperty(ref currentPage, value); }
        }

        private int totalItems;
        public int TotalItems
        {
            get { return totalItems; }
            set { SetProperty(ref totalItems, value); }
        }


        public int PageSize => 21;

        private List<string> SelectedPokemonTypes => PokemonTypes?.Where(x => x.Selected).Select(x => x.Item).ToList();

        #endregion

        #region Commands

        public Command FilterItemsCommand { get; private set; }


        public Command<int> ChangePageCommand { get; private set; }


        public Command<Pokemon> DisplayDetailsCommand { get; private set; }

        #endregion


        public MainViewModel(IPokedexService pokedexService)
        {
            _pokedexService = pokedexService;

            FilterItemsCommand = new Command<SelectableItem<string>>((item) => {
                item.Selected = !item.Selected;
                FilterItems(currentPaged.Results);
            });

            ChangePageCommand = new Command<int>(async (page) => await ChangePageAsync(page));

            DisplayDetailsCommand = new Command<Pokemon>(OpenDetails);
        }
        public override async Task InitializeAsync()
        {
            await List(1, PageSize);
        }

        private async Task ChangePageAsync(int page)
        {
            await List(page, PageSize);
        }

        public async Task List(int page, int size)
        {
            try
            {
                IsBusy = true;

                currentPaged = await _pokedexService.ListAsync(page, size);

                if (currentPaged != null)
                {
                    var types = currentPaged.Results.SelectMany(x => x.Types.Select(t=> t.Type.Name)).Distinct().ToList();


                    IEnumerable<SelectableItem<string>> selectable;

                    if (PokemonTypes != null && PokemonTypes.Any(d=>d.Selected))
                        selectable = types.Select(x => new SelectableItem<string>(x, PokemonTypes.Any(s=> s.Item == x && s.Selected)));
                    else
                        selectable = types.Select(x => new SelectableItem<string>(x));

                    TotalItems = currentPaged.Count;
                    FilterItems(currentPaged.Results);
                    PokemonTypes = new ObservableRangeCollection<SelectableItem<string>>(selectable);

                    CurrentPage = page;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void FilterItems(List<Pokemon> source)
        {
            List<Pokemon> list;
            var selectedTypes = SelectedPokemonTypes?.ToList();

            if (selectedTypes != null && selectedTypes.Count > 0)
                list = source.Where(x => x.Types.Any(t => selectedTypes.Contains(t.Type.Name))).ToList();
            else
                list = source;

            Pokemons = new ObservableRangeCollection<Pokemon>(list);

        }

        private void OpenDetails(Pokemon pokemon)
        {
            NavigationService.NavigateModalAsync<DetailsViewModel>(pokemon);
        }

    }
}
