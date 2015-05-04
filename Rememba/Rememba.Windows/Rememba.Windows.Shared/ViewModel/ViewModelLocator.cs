/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Rememba.Windows"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Rememba.Contracts.Plugins;
using Rememba.Contracts.Services;
using Rememba.Contracts.ViewModels;
using Rememba.Contracts.Views;
using Rememba.Service.Windows.Data;
using Rememba.Service.Windows.Infrastructure;
using Rememba.Shared;
using Rememba.ViewModels.Windows;
using Rememba.Windows.Views;

namespace Rememba.Windows.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var navigationService = this.CreateNavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => navigationService);
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<MainViewModel>();
            //SimpleIoc.Default.Register<SomeObjectDetailViewModel>();
            SimpleIoc.Default.Register<IStateService, StateService>();

            SimpleIoc.Default.Register<ISomeObjectDetailViewModel, SomeObjectDetailViewModel>();
            SimpleIoc.Default.Register<ISomeOtherObjectDetailViewModel, SomeOtherObjectDetailViewModel>();
            SimpleIoc.Default.Register<IMainViewViewModel, MainViewViewModel>();

            //SimpleIoc.Default.Register<INodePlugin, INodePlugin>();

            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    // Create design time view services and models
            //    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            //}
            //else
            //{
                // Create run time view services and models
            SimpleIoc.Default.Register<ISomeDataService, SomeDataService>(); 
            SimpleIoc.Default.Register<IMindMapDataService, MindMapDataService>();
            SimpleIoc.Default.Register<IContentDataService, ContentDataService>();
            //}

            SimpleIoc.Default.Register<ICacheDataService, CacheDataService>();
        }


        public static void SetAndReg()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            
            var navigationService = new NavigationService();
            navigationService.Configure(PageNames.SomeObjectDetailView, typeof(SomeObjectDetailView));
            navigationService.Configure(PageNames.SomeOtherObjectDetailView, typeof(SomeOtherObjectDetailView));
            navigationService.Configure(PageNames.MainView, typeof(MainView));
            SimpleIoc.Default.Register<INavigationService>(() => navigationService);
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<MainViewModel>();
            //SimpleIoc.Default.Register<SomeObjectDetailViewModel>();
            SimpleIoc.Default.Register<IStateService, StateService>();

            SimpleIoc.Default.Register<ISomeObjectDetailViewModel, SomeObjectDetailViewModel>();
            SimpleIoc.Default.Register<ISomeOtherObjectDetailViewModel, SomeOtherObjectDetailViewModel>();
            SimpleIoc.Default.Register<IMainViewViewModel, MainViewViewModel>();

            //SimpleIoc.Default.Register<INodePlugin, INodePlugin>();

            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    // Create design time view services and models
            //    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            //}
            //else
            //{
            // Create run time view services and models
            SimpleIoc.Default.Register<ISomeDataService, SomeDataService>();
            SimpleIoc.Default.Register<IMindMapDataService, MindMapDataService>();
            SimpleIoc.Default.Register<IContentDataService, ContentDataService>();
            //}

            SimpleIoc.Default.Register<ICacheDataService, CacheDataService>();

         
        }

        private INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure(PageNames.SomeObjectDetailView, typeof(SomeObjectDetailView));
            navigationService.Configure(PageNames.SomeOtherObjectDetailView, typeof(SomeOtherObjectDetailView));
            navigationService.Configure(PageNames.MainView, typeof(MainView));


            return navigationService;
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        public ISomeObjectDetailViewModel SomeObjectDetailViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ISomeObjectDetailViewModel>();
            }
        }

        public ISomeOtherObjectDetailViewModel SomeOtherObjectDetailViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ISomeOtherObjectDetailViewModel>();
            }
        }

        public IMainViewViewModel MainViewViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<IMainViewViewModel>();
            }
        }
    }
}