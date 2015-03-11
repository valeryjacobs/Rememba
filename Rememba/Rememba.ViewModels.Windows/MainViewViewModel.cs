using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Rememba.Contracts.Models;
using Rememba.Contracts.Services;
using Rememba.Contracts.ViewModels;
using Rememba.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Rememba.ViewModels.Windows
{
    public class MainViewViewModel : ViewModelBase, IMainViewViewModel
    {
        public RelayCommand GoBack { get; set; }
        //public RelayCommand<SelectionChangedEventArgs> SelectParentNodeCommand { get; set; }
        public RelayCommand<SelectionChangedEventArgs> SelectChildNodeCommand { get; set; }
        public RelayCommand<SelectionChangedEventArgs> SelectSubChildNodeCommand { get; set; }

        public RelayCommand<INode> SelectParentNodeCommand { get; set; }

        private IMindMapDataService _mindMapDataService;
        private IContentDataService _contentDataService;
        private INavigationService _navigationService;


        private ObservableCollection<INode> _parentList;
        private ObservableCollection<INode> _childList;
        private ObservableCollection<INode> _subChildList;
        private IContent _selectedNodeContent;
        private INode _selectedNode;
        private INode _rootNode;

        public async void Initialize(object parameter)
        {
            var mindMap = await _mindMapDataService.GetMindMap("MyKnowledge");
            RootNode = await _mindMapDataService.GetRootNode(mindMap);

            ParentList = RootNode.Children;
            SelectParent(RootNode.Children[2]);
        }

        public MainViewViewModel(
            IMindMapDataService mindMapDataService,
            IContentDataService contentDataService,
            INavigationService navigationService)
        {
            this._mindMapDataService = mindMapDataService;
            this._navigationService = navigationService;
            this._contentDataService = contentDataService;

            InitializeCommands();
            Initialize("");
        }

        private void InitializeCommands()
        {
            GoBack = new RelayCommand(() =>
            {
                _navigationService.GoBack();
            });


            SelectParentNodeCommand = new RelayCommand<INode>(node =>
            {
                if (node == null) return;

                SelectParent(node);
            });
            //SelectParentNodeCommand = new RelayCommand<SelectionChangedEventArgs>((e) =>
            //{
            //    SelectParent((Node)e.AddedItems[0]);
            //});

            SelectChildNodeCommand = new RelayCommand<SelectionChangedEventArgs>((e) =>
            {
                SelectChild((Node)e.AddedItems[0]);
            });
            SelectSubChildNodeCommand = new RelayCommand<SelectionChangedEventArgs>((e) =>
            {
                SelectSubChild((Node)e.AddedItems[0]);
            });
        }



        public INode RootNode
        {
            get { return _rootNode; }
            set
            {
                _rootNode = value;
                RaisePropertyChanged("RootNode");
            }
        }


        public INode SelectedNode
        {
            get
            {
                return _selectedNode;
            }
            set
            {
                _selectedNode = value;
                RaisePropertyChanged("SelectedNode");
            }
        }

        public IContent SelectedNodeContent
        {
            get
            {
                return _selectedNodeContent;
            }
            set
            {
                _selectedNodeContent = value;
                RaisePropertyChanged("SelectedNodeContent");
            }
        }

        private INode _selectedParent;

        public INode SelectedParent
        {
            get { return _selectedParent; }
            set
            {
                SelectParent(value);

                RaisePropertyChanged("SelectedParent");
            }
        }

        //public INode SelectedParent { get; set; }

        private INode _selectedChild;

        public INode SelectedChild
        {
            get { return _selectedChild; }
            set
            {
                SelectChild(value);

                RaisePropertyChanged("SelectedChild");
            }
        }

        private INode _selectedSubChild;

        public INode SelectedSubChild
        {
            get { return _selectedSubChild; }
            set
            {
                SelectSubChild(value);

                RaisePropertyChanged("SelectedSubChild");
            }
        }

        public ObservableCollection<INode> SubChildList
        {
            get
            {
                return _subChildList;
            }
            set
            {
                _subChildList = value;
                RaisePropertyChanged("SubChildList");
            }
        }

        public void SelectChild(INode selectedChild)
        {
            SelectedNode = selectedChild;
            _selectedChild = SelectedNode;
            SubChildList = SelectedChild.Children;
            SetContent();
        }

        public void SelectParent(INode selectedParent)
        {
            SelectedNode = selectedParent;
            _selectedParent = SelectedNode;
            ChildList = SelectedNode.Children;
            if ((SelectedNode.Children != null) && (SelectedNode.Children.Count() > 0))
            {
                SubChildList = SelectedNode.Children[0].Children;
            }
            else
            {
                SubChildList = null;
            }
            SetContent();
        }

        public void SelectSubChild(INode selectedSubChild)
        {
            SelectedNode = selectedSubChild;
            _selectedSubChild = SelectedNode;
            if ((SelectedNode.Children != null) && (SelectedNode.Children.Count() > 0))
            {
                _selectedParent = SelectedChild;
                _selectedChild = selectedSubChild;
                if (SelectedNode.Parent.Parent != null)
                {
                    ParentList = SelectedNode.Parent.Parent.Children;
                }
                ChildList = SelectedNode.Parent.Children;
                SubChildList = SelectedNode.Children;
            }
            SetContent();
        }

        public void SetContent()
        {
            var content = _contentDataService.GetContent(SelectedNode.ContentId);

        }

        public ObservableCollection<INode> ChildList
        {
            get
            {
                return _childList;
            }
            set
            {
                _childList = value;
                RaisePropertyChanged("ChildList");
            }
        }
        public ObservableCollection<INode> ParentList
        {
            get
            {
                return _parentList;
            }
            set
            {
                _parentList = value;
                RaisePropertyChanged("ParentList");
            }
        }
    }
}
