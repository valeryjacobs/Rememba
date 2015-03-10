using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Rememba.Contracts.Models;
using Rememba.Contracts.Services;
using Rememba.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.ViewModels.Windows
{
    public class MainViewViewModel : ViewModelBase, IMainViewViewModel
    {
        public RelayCommand GoBack { get; set; }

        private IMindMapDataService _mindMapDataService;
        private INavigationService _navigationService;

        public async void Initialize(object parameter)
        {
            var mindMap = await _mindMapDataService.GetMindMap("MyKnowledge");
            RootNode = await _mindMapDataService.GetRootNode(mindMap);
        }

        public MainViewViewModel(IMindMapDataService mindMapDataService,
            INavigationService navigationService)
        {
            this._mindMapDataService = mindMapDataService;
            this._navigationService = navigationService;

            InitializeCommands();
            Initialize("");
        }

        private void InitializeCommands()
        {
            GoBack = new RelayCommand(() =>
            {
                _navigationService.GoBack();
            });
        }

        private INode _selectedNode;

        public INode SelectedNode
        {
            get { return _selectedNode; }
            set { _selectedNode = value; }
        }

        private INode _rootNode;

        public INode RootNode
        {
            get { return _rootNode; }
            set
            {
                _rootNode = value;
                RaisePropertyChanged("RootNode");
            }
        }
    }
}
