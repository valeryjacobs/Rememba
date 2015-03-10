using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Rememba.Contracts.Models;
using Rememba.Contracts.Services;
using Rememba.Contracts.ViewModels;
using Rememba.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.ViewModels.Windows
{
    public class SomeObjectDetailViewModel : ViewModelBase, ISomeObjectDetailViewModel
    {
        public RelayCommand GoBack { get; set; }

        private ISomeDataService someDataService;
        private INavigationService navigationService;
        private ISomeObjectDetail selectedObjectDetail;

        string _testBinding = " hoi ";
        public string TestBinding
        {
            get
            {
                return _testBinding;
            }
            set
            {
                _testBinding = value;
                RaisePropertyChanged("TestBinding");
            }
        }

        public ISomeObjectDetail SelectedObject
        {
            get
            {
                return selectedObjectDetail;
            }
            set
            {
                selectedObjectDetail = value;
                RaisePropertyChanged("SelectedObjectDetail");
            }
        }
        public SomeObjectDetailViewModel(ISomeDataService someDataService,
            INavigationService navigationService)
        {
            this.someDataService = someDataService;
            this.navigationService = navigationService;

            InitializeCommands();
            Initialize("");
        }

        private void InitializeCommands()
        {
            GoBack = new RelayCommand(() =>
            {
                navigationService.GoBack();
            });
        }

        public async void Initialize(object parameter)
        {
            SelectedObject =
                await someDataService.GetSomeObjectDetailWithSetOfSomeOtherObject(parameter.ToString());

            //SelectedObject = new SomeObjectDetail()
            //{
            //    SomeDetailProp = "fgdfg",
            //    SomeIntProp = 2,
            //    SomeStringProp = "sdfsdf"
            //};

        }
    }
}
