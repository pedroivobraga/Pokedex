using MvvmHelpers;
using Pokedex.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.ViewModels
{
    public class ViewModelBase : ObservableObject, IViewModel
    {
        public INavigationService NavigationService { get; private set; }


        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public ViewModelBase()
        {
            NavigationService = AppContainer.Current.Resolve<INavigationService>();
            //DialogService = ViewModelLocator.Resolve<IDialogService>();
            //RepositoryService = ViewModelLocator.Resolve<IRepositoryService>();
            //PreferenceService = ViewModelLocator.Resolve<IPreferenceService>();
        }

        //protected INavigationService NavigationService { get; }
        //protected IDialogService DialogService { get; }
        //protected IRepositoryService RepositoryService { get; }
        //protected IPreferenceService PreferenceService { get; }

        public virtual Task InitializeAsync(object param) => Task.CompletedTask;

        public virtual Task InitializeAsync() => InitializeAsync(null);
    }
}
