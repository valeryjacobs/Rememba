using Newtonsoft.Json;
using Rememba.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Model
{
    public class Domain : INotifyPropertyChanged
    {
        private List<Content> _items;
        private List<MindMap> _mindmaps;

        private ObservableCollection<INode> _parentList;
        private ObservableCollection<INode> _childList;
        private ObservableCollection<INode> _subChildList;

        private INode _rootNode;
        private INode _selectedNode;
        private Content _selectedNodeContent;
        private MindMap _currentMindMap;

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddChildNode()
        {
            CreateNode(true, SelectedNode);
        }

        public Content AddContent()
        {
            Content instance = new Content
            {
                Id = Guid.NewGuid().ToString()
            };
            _items.Add(instance);
            return instance;
        }

        public void AddSiblingNode()
        {
            CreateNode(false, SelectedNode);
        }

        private JsonNode BuildJSON(JsonNode jsonNode, INode source)
        {
            if (source == null)
            {
                return null;
            }
            JsonNode node = new JsonNode
            {
                id = source.Id,
                cid = source.ContentId,
                d = source.Description,
                n = source.Title
            };
            if (source.Children != null)
            {
                foreach (INode node2 in source.Children)
                {
                    node.c.Add(BuildJSON(node, node2));
                }
            }
            return node;
        }

        private INode BuildTree(INode node, dynamic source)
        {
            var newNode = new Node
            {
                Id = source.id,
                ContentId = source.cid,
                Description = source.d,
                Title = source.n,
                Parent = node
            };

            if (source.c != null)
            {
                foreach (dynamic d in source.c)
                {
                    newNode.Children.Add(BuildTree(newNode, d));
                }
            }

            return newNode;
        }

        private void CreateNode(bool addAsChild, INode parentNode = null)
        {
            if (parentNode == null)
            {
                parentNode = RootNode;
            }
            INode node = new Node();
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

        public void DeleteNode()
        {
            SelectedNode.Parent.Children.Remove(SelectedNode);
        }

        public void DeleteNode(INode targetNode)
        {
            targetNode.Parent.Children.Remove(targetNode);
        }

        public void EditContent()
        {
            if ((SelectedNode.ContentId == null) || (SelectedNode.ContentId == "1"))
            {
                SelectedNode.ContentId = AddContent().Id;
            }
        }


        public async void Init()
        {
            await InitContentItems();
            await InitMindMaps();
        }


        private async Task InitContentItems()
        {
            //// Retrieve storage account from connection string.
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            //    CloudConfigurationManager.GetSetting("StorageConnectionString"));

            //// Create the blob client.
            //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //// Retrieve reference to a previously created container.
            //CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            //// Retrieve reference to a blob named "myblob".
            //CloudBlockBlob blockBlob = container.GetBlockBlobReference("myblob");

            //// Create or overwrite the "myblob" blob with contents from a local file.
            //using (var fileStream = System.IO.File.OpenRead(@"path\myfile"))
            //{
            //    blockBlob.UploadFromStream(fileStream);
            //} 
            // _items = await            
        }

        private async Task InitMindMaps()
        {
            //    _mindmaps = await 
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

        public void NavigateUp()
        {
            SubChildList = SelectedSubChild.Parent.Children;
            ChildList = SelectedSubChild.Parent.Parent.Children;
            ParentList = SelectedSubChild.Parent.Parent.Parent.Children;
            SelectedSubChild = SelectedSubChild.Parent;
            SelectedChild = SelectedChild.Parent;
            SelectedParent = SelectedParent.Parent;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async void Save()
        {
            JsonNode jsonNode = new JsonNode();
            JsonNode jsonObject = BuildJSON(jsonNode, RootNode);
            MindMap instance = new MindMap
            {
                Name = CurrentMindMap.Name + "_BACKUP",
                Id = Guid.NewGuid().ToString(),
                Content = CurrentMindMap.Content
            };

            CurrentMindMap.Content = JsonConvert.SerializeObject(jsonObject);
            UpdateMindMap(CurrentMindMap);
        }

        public void SelectChild(INode selectedChild)
        {
            SelectedNode = selectedChild;
            SelectedChild = SelectedNode;
            SubChildList = SelectedChild.Children;
            SetContent();
        }

        public void SelectParent(INode selectedParent)
        {
            SelectedNode = selectedParent;
            SelectedParent = SelectedNode;
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
            SelectedSubChild = SelectedNode;
            if ((SelectedNode.Children != null) && (SelectedNode.Children.Count() > 0))
            {
                SelectedParent = SelectedChild;
                SelectedChild = selectedSubChild;
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
            Func<Content, bool> func = null;
            if (Enumerable.Any<Content>((IEnumerable<Content>)_items, delegate(Content x)
            {
                return x.Id == SelectedNode.ContentId;
            }))
            {
                if (func == null)
                {
                    func = delegate(Content x)
                    {
                        return x.Id == SelectedNode.ContentId;
                    };
                }
                SelectedNodeContent = Enumerable.Single<Content>(Enumerable.Where<Content>((IEnumerable<Content>)_items, func));
            }
            else
            {
                Content content = new Content
                {
                    Id = SelectedNode.ContentId,
                    Data = "Content with ID " + SelectedNode.ContentId + " not found."
                };
                SelectedNodeContent = content;
            }
        }

        public async void UpdateContent(string newContent)
        {
            SelectedNodeContent.Data = newContent;
            //await _contentTable.UpdateAsync(SelectedNodeContent);
        }

        private async void UpdateMindMap(MindMap mindmap)
        {
            //  await _mindmapTable.UpdateAsync(mindmap);
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
                NotifyPropertyChanged("ChildList");
            }
        }

        public MindMap CurrentMindMap
        {
            get
            {
                return _currentMindMap;
            }
            set
            {
                _currentMindMap = value;
                NotifyPropertyChanged("CurrentMindMap");
            }
        }

        public string InitializationStatus { get; set; }

        public bool Initializing { get; set; }

        public bool IsChildSelected { get; set; }

        public bool IsParentSelected { get; set; }

        public bool IsSubChildSelected { get; set; }

        public ObservableCollection<INode> ParentList
        {
            get
            {
                return _parentList;
            }
            set
            {
                _parentList = value;
                NotifyPropertyChanged("ParentList");
            }
        }

        public INode RootNode
        {
            get
            {
                return _rootNode;
            }
            set
            {
                _rootNode = value;
                NotifyPropertyChanged("RootNode");
            }
        }

        private double _zoom;
        public double Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                _zoom = value;
                NotifyPropertyChanged("Zoom");
            }
        }

        public INode SelectedChild { get; set; }

        public INode SelectedNode
        {
            get
            {
                return _selectedNode;
            }
            set
            {
                _selectedNode = value;
                NotifyPropertyChanged("SelectedNode");
            }
        }

        public Content SelectedNodeContent
        {
            get
            {
                return _selectedNodeContent;
            }
            set
            {
                _selectedNodeContent = value;
                NotifyPropertyChanged("SelectedNodeContent");
            }
        }

        public INode SelectedParent { get; set; }

        public INode SelectedSubChild { get; set; }

        public ObservableCollection<INode> SubChildList
        {
            get
            {
                return _subChildList;
            }
            set
            {
                _subChildList = value;
                NotifyPropertyChanged("SubChildList");
            }
        }

        // initcontentitems()

        // props
        // selectedparent etc

        // editnode

        // move up

        // na toevoegen async commando gebruiken om prop changed property op de ui thread te updaten

        // artikel over binding 

        //selectednode

        //parentlist etc

        //    movemup

        //    save




    }
}
