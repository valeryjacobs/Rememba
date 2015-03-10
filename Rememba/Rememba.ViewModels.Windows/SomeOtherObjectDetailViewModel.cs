using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Rememba.Contracts.Models;
using Rememba.Contracts.Services;
using Rememba.Contracts.ViewModels;
using Rememba.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rememba.ViewModels.Windows
{
    public class SomeOtherObjectDetailViewModel : ViewModelBase, ISomeOtherObjectDetailViewModel
    {
        public RelayCommand GoBack { get; set; }
        public RelayCommand GoHome { get; set; }
        public RelayCommand AddToFavorites { get; set; }


        private ISomeDataService someDataService;
        private INavigationService navigationService;
        private IDialogService dialogService;

        private IStateService stateService;

        private ISomeOtherObjectDetail selectedSomeOtherObjectDetail;
        public ISomeOtherObjectDetail SelectedSomeOtherObjectDetail
        {
            get
            {
                return selectedSomeOtherObjectDetail;
            }
            set
            {
                selectedSomeOtherObjectDetail = value;

                //tileService.SendSimpleTextUpdate
                //   (SelectedSomeOtherObjectDetail.TravelName);
                //tileService.SendImageAndTextUpdate(SelectedTravelDetail.TravelName, SelectedTravelDetail.ImageUrl);
                //toastService.SendImageAndTextToast(SelectedTravelDetail.TravelName, SelectedTravelDetail.ImageUrl);

                stateService.AddState(PageNames.SomeOtherObjectDetailView,
                    selectedSomeOtherObjectDetail.SomeIntProp.ToString());
                RaisePropertyChanged("SelectedSomeOtherObjectDetail");
            }
        }

        public SomeOtherObjectDetailViewModel(ISomeDataService someDataService, INavigationService navigationService, IDialogService dialogService, IStateService stateService)
        {
            this.someDataService = someDataService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;

            //this.tileService = tileService;
            //this.toastService = toastService;
            this.stateService = stateService;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            GoBack = new RelayCommand(() =>
            {
                navigationService.GoBack();
            });
            GoHome = new RelayCommand(() =>
            {
                navigationService.NavigateTo(PageNames.SomeObjectDetailView);
            });
            AddToFavorites = new RelayCommand(() =>
            {
                dialogService.ShowMessage("test","test");
            });
        }

        public async void Initialize(object parameter)
        {
            SelectedSomeOtherObjectDetail = await someDataService.GetSomeOtherObjectDetails(parameter.ToString());
        }
    }
}
