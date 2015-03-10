using Rememba.Contracts.Services;
using Rememba.Contracts.ViewModels;
using Rememba.Contracts.Views;
using Rememba.Service.Windows.Data;
using Rememba.Service.Windows.Infrastructure;
using Rememba.Shared;
using Rememba.ViewModels.Windows;
using Rememba.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            //ViewModel registration

            InstanceFactory.RegisterType<ISomeOtherObjectDetailViewModel, SomeOtherObjectDetailViewModel>();
            InstanceFactory.RegisterType<ISomeObjectDetailViewModel, SomeObjectDetailViewModel>();


            ////View registration

            InstanceFactory.RegisterType<ISomeOtherObjectDetailView, SomeOtherObjectDetailView>();
            InstanceFactory.RegisterType<ISomeObjectDetailView, SomeObjectDetailView>();


            ////Services registration
            InstanceFactory.RegisterType<ISomeDataService,
                SomeDataService>();
            InstanceFactory.RegisterType<INavigationService,
                NavigationService>();
            InstanceFactory.RegisterType<IDialogService, DialogService>();

            //InstanceFactory.RegisterType<IPushNotificationService, PushNotificationService>();
            //InstanceFactory.RegisterType<ITileService, TileService>();
            //InstanceFactory.RegisterType<IToastService, ToastService>();
            InstanceFactory.RegisterType<IStateService, StateService>();
            InstanceFactory.RegisterType<ICacheDataService, CacheDataService>();
        }

        public ISomeOtherObjectDetailViewModel SomeOtherObjectDetailViewModel
        {
            get
            {
                return InstanceFactory.GetInstance<ISomeOtherObjectDetailViewModel>();
            }
        }

        public ISomeObjectDetailViewModel SomeObjectDetailViewModel
        {
            get
            {
                return InstanceFactory.GetInstance<ISomeObjectDetailViewModel>();
            }
        }


    }
}
