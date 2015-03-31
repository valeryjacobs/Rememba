﻿using GalaSoft.MvvmLight;
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
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Rememba.ViewModels.Windows
{
    public class MainViewViewModel : ViewModelBase, IMainViewViewModel
    {
        public RelayCommand GoDo { get; set; }
        public RelayCommand Order { get; set; }
        public RelayCommand ClearLocalCacheCommand { get; set; }
        public RelayCommand Copy { get; set; }
        public RelayCommand PasteChild { get; set; }
        public RelayCommand PasteSibling { get; set; }
        public RelayCommand LoadGraphCommand { get; set; }
        public RelayCommand DeleteGraphCommand { get; set; }
        public RelayCommand CreateGraphCommand { get; set; }
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
        public RelayCommand EditNodeCommand { get; set; }
        public RelayCommand EditContent { get; set; }
        public RelayCommand Save { get; set; }
        //public RelayCommand<SelectionChangedEventArgs> SelectParentNodeCommand { get; set; }
        public RelayCommand<SelectionChangedEventArgs> SelectChildNodeCommand { get; set; }
        public RelayCommand<SelectionChangedEventArgs> SelectSubChildNodeCommand { get; set; }
        public RelayCommand<INode> SelectParentNodeCommand { get; set; }

        private IMindMapDataService _mindMapDataService;
        private IContentDataService _contentDataService;
        private INavigationService _navigationService;
        private IDialogService _dialogService;
        private ICacheDataService _cacheService;


        private ObservableCollection<INode> _parentList;
        private ObservableCollection<INode> _childList;
        private ObservableCollection<INode> _subChildList;
        private IContent _selectedNodeContent;
        private INode _selectedNode;
        private INode _rootNode;
        private IMindMap _mindMap;

        public IMindMap MindMap
        {
            get { return _mindMap; }
            set
            {
                _mindMap = value;
                RaisePropertyChanged("MindMap");
            }
        }

        private bool _newGraphMode;

        public bool NewGraphMode
        {
            get { return _newGraphMode; }
            set { _newGraphMode = value; }
        }


        private string _newGraphName;

        public string NewGraphName
        {
            get { return _newGraphName; }
            set
            {
                _newGraphName = value;
                RaisePropertyChanged("NewGraphName");
            }
        }


        private async Task InitMindMap()
        {
            if (_mindMap.Content == null)
            {
                _mindMap = await _mindMapDataService.GetMindMap(_mindMap.Id);
            }

            RootNode = await _mindMapDataService.GetRootNode(_mindMap);

            ParentList = RootNode.Children;
            if (RootNode.Children.Count > 0)
            {
                SelectParent(RootNode.Children[0]);
            }
        }

        public INode PreviousSelectedNode { get; set; }


        public async void Initialize(object parameter)
        {
            Graphs = await _mindMapDataService.ListMindMaps();
            if (Graphs.Count > 0)
            {
                //MindMap = Graphs.First();
                //await InitMindMap();
            }
        }

        public MainViewViewModel(
            IMindMapDataService mindMapDataService,
            IContentDataService contentDataService,
            INavigationService navigationService,
            IDialogService dialogService,
            ICacheDataService cacheService)
        {
            this._mindMapDataService = mindMapDataService;
            this._navigationService = navigationService;
            this._contentDataService = contentDataService;
            this._dialogService = dialogService;
            this._cacheService = cacheService;


            InitializeCommands();
            Initialize("");
        }

        private INode _clipBoardNode;

        public INode ClipBoardNode
        {
            get { return _clipBoardNode; }
            set { _clipBoardNode = value; }
        }


        private async void InitializeCommands()
        {
            ClearLocalCacheCommand = new RelayCommand(async () => {

                  await _dialogService.ShowMessage("Do you want to clear the local cache? All unsynced changes will be lost. FOREVER!!",
                        "Warning",
                        buttonConfirmText: "Yes", buttonCancelText: "No",
                        afterHideCallback:  (confirmed) =>
                        {
                            if (confirmed)
                            {
                               _cacheService.ClearCache();
                            }
                        });
            });

            Order = new RelayCommand(() => {
                if(ChildList.Contains(SelectedNode))
                { 
                   ChildList =  ChildList.OrderBy(x => x.Title) as ObservableCollection<INode>;
                }

                if (SubChildList.Contains(SelectedNode))
                {
                    SubChildList = SubChildList.OrderBy(x => x.Title) as ObservableCollection<INode>;
                }

                if (ParentList.Contains(SelectedNode))
                {
                    ParentList = ParentList.OrderBy(x => x.Title) as ObservableCollection<INode>;
                }

                //SelectedNode.Parent.Children.OrderBy(x => x.Title);
            });

            Copy = new RelayCommand(() =>
            {
                ClipBoardNode = SelectedNode;
            });

            PasteChild = new RelayCommand(async () =>
            {
                ClipBoardNode = await _mindMapDataService.CloneNode(ClipBoardNode); 

                SelectedNode.Children.Add(ClipBoardNode);
                ClipBoardNode.Parent.Children.Remove(ClipBoardNode);

            });

            PasteSibling = new RelayCommand(() =>
            {
                SelectedNode.Parent.Children.Add(ClipBoardNode);
                ClipBoardNode.Parent.Children.Remove(ClipBoardNode);

            });

            LoadGraphCommand = new RelayCommand(async () =>
            {
                await InitMindMap();

            });

            GoDo = new RelayCommand(async () =>
            {
                //  Graphs = await _mindMapDataService.ListMindMaps();
                await _contentDataService.ClearCache();

            });

            CreateGraphCommand = new RelayCommand(async () =>
            {
                if (_mindMap != null && _rootNode != null)
                {
                    await _dialogService.ShowMessage("Do you want to save the current graph? All intermediate changes will be lost.",
                        "Warning",
                        buttonConfirmText: "Yes", buttonCancelText: "No",
                        afterHideCallback: async (confirmed) =>
                        {
                            if (confirmed && _mindMap != null && _rootNode != null)
                            {
                                await _mindMapDataService.Save(_mindMap, _rootNode);
                            }


                            await CreateGraph();
                        });
                }
                else
                {
                    await CreateGraph();
                }
            }); 

            DeleteGraphCommand = new RelayCommand(async () =>
            {
                await  _dialogService.ShowMessage("Do you want to dalete the current graph? [" + MindMap.Name + "]",
                    "Warning",
                    buttonConfirmText: "Yes", buttonCancelText: "No",
                    afterHideCallback: async (confirmed) =>
                    {
                        if (confirmed)
                        {
                            _graphs.Remove(MindMap);
                            await _mindMapDataService.Delete(MindMap.Id);
                            RootNode = null;
                        }
                    });
               
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

            DeleteContent = new RelayCommand(async () =>
            {
                await _dialogService.ShowMessage("Are you sure you want to delete this nodes content? [" + SelectedNode.Title + "]",
                    "Warning",
                    buttonConfirmText: "Yes", buttonCancelText: "No",
                    afterHideCallback: async (confirmed) =>
                    {
                        if (confirmed)
                        {
                            await _contentDataService.DeleteContent(SelectedNodeContent.Id);
                            SelectedNodeContent.Data = "";
                            SelectedNode.ContentId = null;
                        }
                    });

            });

            EditContent = new RelayCommand(() =>
            {

            });

            DeleteNodeCommand = new RelayCommand(async () =>
            {
                await _dialogService.ShowMessage("Are you sure you want to delete this node? [" + SelectedNode.Title + "]",
                   "Warning",
                   buttonConfirmText: "Yes", buttonCancelText: "No",
                   afterHideCallback: (confirmed) =>
                   {
                       if (confirmed)
                       {
                           DeleteNode();
                       }
                   });

            });

            AddChildNodeCommand = new RelayCommand(() =>
            {
                CreateNode(true, SelectedNode);
            });

            AddSiblingNodeCommand = new RelayCommand(() =>
            {
                CreateNode(false, SelectedNode);
            });

            EditNodeCommand = new RelayCommand(() =>
            {
                SwitchNodeToEditMode();
            });
        }

        private async Task CreateGraph()
        {
            _mindMap = await _mindMapDataService.Create(NewGraphName);

            RootNode = await _mindMapDataService.GetRootNode(_mindMap);

            ParentList = RootNode.Children;
            if (RootNode.Children.Count > 0)
            {
                SelectParent(RootNode.Children[0]);

            }

            Initialize(null);
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

  

        private void SwitchNodeToEditMode()
        {
            if (PreviousSelectedNode != null)
            {
                PreviousSelectedNode.Edit = false;
            }

            SelectedNode.Edit = !SelectedNode.Edit;
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
                PreviousSelectedNode = _selectedNode;
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
            SwitchNodeToEditMode();
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

        private List<IMindMap> _graphs;

        public List<IMindMap> Graphs
        {
            get { return _graphs; }
            set
            {
                _graphs = value;
                RaisePropertyChanged("Graphs");

            }
        }

    }
}
