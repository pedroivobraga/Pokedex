using Pokedex.Models;
using System.Threading.Tasks;

namespace Pokedex.Service
{
    public interface IPokedexService
    {
        Task<Paged<Pokemon>> ListAsync(int page, int size);
    }
}
