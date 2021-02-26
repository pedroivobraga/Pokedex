using MonkeyCache.SQLite;
using Pokedex.Navigation;
using Pokedex.ViewModels;
using Pokedex.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pokedex
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var navigationService = AppContainer.Current.Resolve<INavigationService>();

            Barrel.ApplicationId = "PokedexCache";

            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                await navigationService.SetRootAsync<MainViewModel>();
            });

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
