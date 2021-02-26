using System.Threading.Tasks;

namespace Pokedex.ViewModels
{
    internal interface IViewModel
    {
        Task InitializeAsync();
    }
}