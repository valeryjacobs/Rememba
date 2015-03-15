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

        public RelayCommand GoBackTree { get; set; }
        public RelayCommand GoUpTree { get; set; }
        public RelayCommand AddContent { get; set; }
        public RelayCommand UpdateContent { get; set; }
        public RelayCommand DeleteContent { get; set; }

        public RelayCommand Save { get; set; }
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

        private IMindMap _mindMap;

        public async void Initialize(object parameter)
        {
            _mindMap = await _mindMapDataService.GetMindMap("MyKnowledge");
            RootNode = await _mindMapDataService.GetRootNode(_mindMap);

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

            GoUpTree = new RelayCommand(() =>
            {
                NavigateUp();
            });

            Save = new RelayCommand(() =>
            {
                _mindMapDataService.Save(_mindMap, _rootNode);
            });

            AddContent = new RelayCommand(() =>
            {
                _contentDataService.AddContent(SelectedNodeContent);
            });

            UpdateContent = new RelayCommand(() =>
            {
                _contentDataService.UpdateContent(SelectedNodeContent);
            });

            DeleteContent = new RelayCommand(() =>
            {
                _contentDataService.DeleteContent(SelectedNodeContent.Id);
                SelectedNodeContent.Data = "";
            });
        }

        public void UpdateContentFromWebView(string content)
        {
            SelectedNodeContent.Data = content;
            _contentDataService.UpdateContent(SelectedNodeContent);
        }
       

        public void NavigateUp()
        {
            if (SelectedSubChild == null || SelectedChild == null || SelectedParent == null || SelectedParent.Parent == null) return;

            SubChildList = SelectedSubChild.Parent.Children;
            ChildList = SelectedSubChild.Parent.Parent.Children;
            ParentList = SelectedSubChild.Parent.Parent.Parent.Children;
            SelectedSubChild = SelectedSubChild.Parent;
            SelectedChild = SelectedChild.Parent;
            SelectedParent = SelectedParent.Parent;
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
            if (selectedChild == null)
            {
                if (_selectedSubChild != null && _selectedSubChild.Parent != null)
                {
                    selectedChild = _selectedSubChild.Parent;
                }
                else
                { return; }
            }

            SelectedNode = selectedChild;
            _selectedChild = SelectedNode;
            SubChildList = SelectedChild.Children;
            SetContent();
        }

        public void SelectParent(INode selectedParent)
        {
            if (selectedParent == null) selectedParent = _selectedChild.Parent;

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
            if (selectedSubChild == null) return;

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

        public async void SetContent()
        {
            var content = await _contentDataService.GetContent(SelectedNode.ContentId);

            SelectedNodeContent = content;

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
