using Pokedex.ViewModels;
using Pokedex.Views;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Pokedex.Navigation
{
    public interface INavigationService
    {
        Task SetRootAsync<TViewModel>() where TViewModel : ViewModelBase;

        Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;
        Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;
        Task NavigateToAsync(Type viewModelType);
        Task NavigateToAsync(Type viewModelType, object parameter);
        Task CloseModalAsync();

        Task NavigateModalAsync<T>(bool animated = true);
        Task NavigateModalAsync<T>(object parameter, bool animated = true);

    }

    public class NavigationService : INavigationService
    {
        protected Application CurrentApplication
        {
            get { return Application.Current; }
        }

        public async Task SetRootAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            Page page = CreateAndBindPage(typeof(TViewModel));

            CurrentApplication.MainPage = page;

            await (page.BindingContext as ViewModelBase).InitializeAsync();
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToAsync(Type viewModelType)
        {
            return InternalNavigateToAsync(viewModelType, null);
        }

        public Task NavigateToAsync(Type viewModelType, object parameter)
        {
            return InternalNavigateToAsync(viewModelType, parameter);
        }


        public Task NavigateModalAsync<T>(bool animated = true)
        {
            return NavigateModalAsync<T>(null, animated);
        }

        public async Task NavigateModalAsync<T>(object parameter, bool animated = true)
        {
            Page page = CreateAndBindPage(typeof(T));

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
            await CurrentApplication.MainPage.Navigation.PushModalAsync(page, animated);

        }
        public Task CloseModalAsync()
        {
            return CurrentApplication.MainPage.Navigation.PopModalAsync();
        }

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreateAndBindPage(viewModelType);

            if (page is MainView)
            {
                CurrentApplication.MainPage = page;
            }
            else if (CurrentApplication.MainPage is MainView)
            {
                var mainPage = CurrentApplication.MainPage as MainView;
                var navigationPage = mainPage.Navigation as NavigationPage;

                if (navigationPage != null)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    navigationPage = new NavigationPage(page);
                }
            }
            else
            {
                var navigationPage = CurrentApplication.MainPage as NavigationPage;

                if (navigationPage != null)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    CurrentApplication.MainPage = new NavigationPage(page);
                }
            }

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        protected Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace(".ViewModels.", ".Views.").TrimEnd("Model".ToCharArray());
            var viewAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewFullName = $"{viewName}, {viewAssemblyName}";

            var viewType = Type.GetType(viewFullName);

            return viewType;
        }

        protected Page CreateAndBindPage(Type viewModelType)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Mapping type for {viewModelType} is not a page");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            ViewModelBase viewModel = AppContainer.Current.Resolve(viewModelType) as ViewModelBase;
            page.BindingContext = viewModel;

            return page;
        }

    }

}
