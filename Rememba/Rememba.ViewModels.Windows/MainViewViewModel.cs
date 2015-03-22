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
        public RelayCommand GoDo { get; set; }
        public RelayCommand GoBack { get; set; }
        public RelayCommand GoBackTree { get; set; }
        public RelayCommand GoUpTree { get; set; }
        public RelayCommand MoveUpOrder { get; set; }
        public RelayCommand MoveDownOrder { get; set; }
        public RelayCommand MoveUpTreeCommand { get; set; }
        public RelayCommand AddContent { get; set; }
        public RelayCommand UpdateContent { get; set; }
        public RelayCommand DeleteContent { get; set; }
        public RelayCommand AddChildNodeCommand { get; set; }
        public RelayCommand AddSiblingNodeCommand { get; set; }
        public RelayCommand DeleteNodeCommand { get; set; }
        public RelayCommand EditContent { get; set; }
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
            _mindMap = await _mindMapDataService.GetMindMap("ValeryJacobs");
            RootNode = await _mindMapDataService.GetRootNode(_mindMap);

            ParentList = RootNode.Children;
            SelectParent(RootNode.Children[0]);
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
            GoDo = new RelayCommand(() =>
            {
                
            });

            GoBack = new RelayCommand(() =>
            {
                _navigationService.GoBack();
            });

            GoUpTree = new RelayCommand(() =>
            {
                NavigateUp();
            });

            MoveUpOrder = new RelayCommand(() =>
            {
                MoveOrderUp();
            });

            MoveDownOrder = new RelayCommand(() =>
            {
                MoveOrderDown();
            });

            MoveUpTreeCommand = new RelayCommand(() =>
            {
                MoveUp();
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

            EditContent = new RelayCommand(() =>
            {

            });

            DeleteNodeCommand = new RelayCommand(() =>
            {
                DeleteNode();
            });

            AddChildNodeCommand = new RelayCommand(() =>
            {
                CreateNode(true, SelectedNode);
            });

            AddSiblingNodeCommand = new RelayCommand(() =>
            {
                CreateNode(false, SelectedNode);
            });
        }

        public void UpdateContentFromWebView(string content)
        {
            if (content == SelectedNodeContent.Data) return;

            if (SelectedNodeContent.Id == "1")
            {
                SelectedNodeContent.Id = Guid.NewGuid().ToString();
            }
            SelectedNodeContent.Data = content;
            _contentDataService.UpdateContent(SelectedNodeContent);

            SelectedNode.ContentId = SelectedNodeContent.Id;

        }

        public void Sort()
        {
            ParentList.OrderBy(x => x.Title);
        }


        public void NavigateUp()
        {
            if (SelectedSubChild == null || SelectedChild == null || SelectedParent == null || SelectedParent.Parent == null) return;

            if (SelectedSubChild.Parent.Parent.Parent.Children == null || SelectedSubChild.Parent.Parent.Parent.Children.Count == 0) return;

            SubChildList = SelectedSubChild.Parent.Children;
            ChildList = SelectedSubChild.Parent.Parent.Children;
            ParentList = SelectedSubChild.Parent.Parent.Parent.Children;
            SelectedSubChild = SelectedSubChild.Parent;
            SelectedChild = SelectedChild.Parent;
            SelectedParent = SelectedParent.Parent;
        }

        public void DeleteNode()
        {
            SelectedNode.Parent.Children.Remove(SelectedNode);
        }

        public void DeleteNode(INode targetNode)
        {
            targetNode.Parent.Children.Remove(targetNode);
        }

        public void DeleteNode(INode targetNode, bool removeContent)
        {
            targetNode.Parent.Children.Remove(targetNode);

            if (removeContent) _contentDataService.DeleteContent(targetNode.ContentId);
        }

        public void MoveOrderDown()
        {
            MoveOrderDown(SelectedNode);
        }

        public void MoveOrderDown(INode targetNode)
        {
            int index = targetNode.Parent.Children.IndexOf(targetNode);
            if (index < (Enumerable.Count<INode>((IEnumerable<INode>)targetNode.Parent.Children) - 1))
            {
                targetNode.Parent.Children.Move(index, index + 1);
            }
        }

        public void MoveOrderUp()
        {
            MoveOrderUp(SelectedNode);
        }

        public void MoveOrderUp(INode targetNode)
        {
            int index = targetNode.Parent.Children.IndexOf(targetNode);
            if (index > 0)
            {
                targetNode.Parent.Children.Move(index, index - 1);
            }
        }

        public void MoveUp()
        {
            if (SelectedNode.Parent.Parent == null) return;

            SelectedNode.Parent.Parent.Children.Add(SelectedNode);
            SelectedNode.Parent.Children.Remove(SelectedNode);
            SelectedNode.Parent = SelectedNode.Parent.Parent;
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
            

            if (selectedParent == null && _selectedChild != null) selectedParent = _selectedChild.Parent;



            SelectedNode = selectedParent;
            _selectedParent = SelectedNode;

            if (SelectedNode == null) return;

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

        private void CreateNode(bool addAsChild, INode parentNode = null)
        {
            if (parentNode == null)
            {
                parentNode = RootNode;
            }
            INode node = new Node();
            node.Title = "New Node added on " + DateTime.Now.ToString();
            if (addAsChild)
            {
                node.Parent = parentNode;
                parentNode.Children.Add(node);
            }
            else
            {
                if (parentNode.Parent == null)
                {
                    return;
                }
                node.Parent = parentNode.Parent;
                parentNode.Parent.Children.Insert(parentNode.Parent.Children.IndexOf(parentNode) + 1, node);
            }
            SelectedNode = node;
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
