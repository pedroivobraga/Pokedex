using Pokedex.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Pokedex.ViewModels
{
    public class DetailsViewModel : ViewModelBase, IViewModel
    {


        private Pokemon pokemon;
        public Pokemon Pokemon
        {
            get { return pokemon; }
            set { SetProperty(ref pokemon, value); }
        
        }


        private MvvmHelpers.ObservableRangeCollection<string> sprites;
        public MvvmHelpers.ObservableRangeCollection<string> Sprites
        {
            get { return sprites; }
            set { SetProperty(ref sprites, value); }
        }

        public Command CloseCommand { get; private set; }


        public DetailsViewModel()
        {
            CloseCommand = new Command(async () => await CloseAsync());
        }

        private async Task CloseAsync()
        {
            await NavigationService.CloseModalAsync();
        }

        public override Task InitializeAsync(object param)
        {
            Pokemon = param as Pokemon;
            Sprites = new MvvmHelpers.ObservableRangeCollection<string>();
            var properties = Pokemon.Sprites.GetType().GetProperties();

            Sprites.Add(Pokemon.Sprites.FrontDefault);

            foreach (var property in properties)
            {
                var value = property.GetValue(pokemon.Sprites) as string;

                if (value != null && property.Name != nameof(PokemonSprites.FrontDefault))
                    Sprites.Add(value);
            }

            return Task.CompletedTask;
        }
    }
}
