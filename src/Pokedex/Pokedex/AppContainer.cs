using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Pokedex.Navigation;
using Pokedex.Service;
using Pokedex.ViewModels;
using Pokedex.Views;

namespace Pokedex
{
    public class AppContainer
    {
        private static AppContainer current;
        public static AppContainer Current
        {
            get
            {
                if (current == null)
                    current = new AppContainer();

                return current;
            }
        }


        private IContainer container;
        private AppContainer()
        {
            // services
            var builder = new ContainerBuilder();
            
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            //builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            //builder.RegisterType<RepositoryService>().As<IRepositoryService>().SingleInstance();
            //builder.RegisterType<PreferenceService>().As<IPreferenceService>().SingleInstance();

            //// view models
            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<DetailsViewModel>().SingleInstance();
            //builder.RegisterType<ActivitiesViewModel>().SingleInstance();
            //builder.RegisterType<ActivityRecordViewModel>().SingleInstance();
            //builder.RegisterType<HistoryViewModel>().SingleInstance();


            builder.RegisterType<PokedexService>().As<IPokedexService>().InstancePerLifetimeScope();

            container = builder.Build();
        }

        public T Resolve<T>() => container.Resolve<T>();
        public T Resolve<T>(object param) => container.Resolve<T>(new TypedParameter(param.GetType(), param));

        public object Resolve(Type type) => container.Resolve(type);
    }
}
