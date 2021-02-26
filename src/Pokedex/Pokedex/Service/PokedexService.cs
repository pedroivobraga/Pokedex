using Pokedex.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.Service
{
    public class PokedexService : BaseService, IPokedexService
    {
        protected string Api => "pokemon";


        public async Task<Paged<Pokemon>> ListAsync(int page, int size)
        {
            var pagedResource = await GetPagedAsync<NamedAPIResource>($"{BaseUri}/{Api}", page, size);
            var paged = new Paged<Pokemon>();

            if (pagedResource != null && pagedResource.Results != null)
            {
                var tasks = pagedResource.Results.Select(resource => GetAsync($"{BaseUri}/{Api}/{resource.Name?.ToLowerInvariant()}"));

                var pokemons = await Task.WhenAll(tasks);

                paged.Results.AddRange(pokemons.OrderBy(x => x.Id));

                paged.Count = pagedResource.Count;
            }

            return paged;

        }

        public async Task<Pokemon> GetAsync(string url)
        {
            try
            {
                return await GetAsync<Pokemon>(url);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }

    }
}
