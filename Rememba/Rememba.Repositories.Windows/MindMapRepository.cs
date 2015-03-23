﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rememba.Contracts.Models;
using Rememba.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.Storage.FileProperties;


namespace Rememba.Repositories.Windows
{
    public class MindMapRepository
    {
        private const string MindmapDataFileName = "initmindmap.json";

        public static bool IsConnected()
        {
            //return false;
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }
        public async Task<Dictionary<string, string>> ListMindMaps()
        {
            var mindMaps = new Dictionary<string, string>();

            CloudBlobContainer container = new CloudBlobContainer(new Uri(Settings.TenantGraphContainerSaS));

            BlobContinuationToken token = null;
            do
            {
                BlobResultSegment results = await container.ListBlobsSegmentedAsync(null, true, BlobListingDetails.None, 1, token, null, null);

                foreach (IListBlobItem blobItem in results.Results)
                {
                    var blob = blobItem.Container.GetBlockBlobReference(blobItem.Uri.Segments[2]);
                    await blob.FetchAttributesAsync();
                    if (blob.Metadata.ContainsKey("Name"))
                    {
                        mindMaps.Add(blob.Metadata["Name"], blobItem.Uri.AbsoluteUri);
                    }
                }

                token = results.ContinuationToken;
            }
            while (token != null);


            return mindMaps;


            ////{"id":null,"n":"aap","d":null,"cid":null,"c":[{"id":null,"n":"Azure Features","d":"A to Z featureset","cid":"ad5372b4-0602-4fbd-b0a6-4c576755a473","c":[{"id":null,"n":"Compute & Networking","d":"","cid":"1","c":[{"id":null,"n":"Virtual Machines","d":null,"cid":"1","c":[{"id":null,"n":"Demos","d":null,"cid":"1","c":[]},{"id":null,"n":"Links and Documents","d":null,"cid":"1","c":[]},{"id":null,"n":"Pricing","d":null,"cid":"1","c":[]}]},{"id":null,"n":"Azure RemoteApp","d":null,"cid":"1","c":[{"id":null,"n":"Demos","d":null,"cid":"1","c":[]}]},{"id":null,"n":"Cloud Services","d":null,"cid":"1","c":[{"id":null,"n":"Demos","d":null,"cid":"1","c":[]}]},{"id":null,"n":"Virtual Networks","d":null,"cid":"1","c":[{"id":null,"n":"Demos","d":null,"cid":"1","c":[]}]},{"id":null,"n":"ExpressRoute","d":null,"cid":"1","c":[{"id":null,"n":"Demos","d":null,"cid":"1","c":[]}]},{"id":null,"n":"Traffic Manager","d":null,"cid":"1","c":[]}]},{"id":null,"n":"Web & Mobile","d":null,"cid":"1","c":[{"id":null,"n":"Websites","d":null,"cid":"1","c":[{"id":null,"n":"Best practices","d":null,"cid":"1","c":[]},{"id":null,"n":"Demos","d":null,"cid":"1","c":[]},{"id":null,"n":"Techniques","d":null,"cid":"1","c":[]}]},{"id":null,"n":"Mobile Services","d":null,"cid":"1","c":[{"id":null,"n":"Techniques","d":"","cid":"1","c":[{"id":null,"n":"Using Azure Storage with Mobile Services","d":"http://chrisrisner.com/Mobile-Services-and-Windows-Azure-Storage","cid":"1","c":[]}]},{"id":null,"n":"Best practices","d":null,"cid":"1","c":[]},{"id":null,"n":"Demos","d":null,"cid":"1","c":[]}]},{"id":null,"n":"API Management","d":null,"cid":"1","c":[{"id":null,"n":"Demos","d":null,"cid":"1","c":[]},{"id":null,"n":"Best practices","d":null,"cid":"1","c":[]}]},{"id":null,"n":"Notification Hubs","d":null,"cid":"1","c":[{"id":null,"n":"Demos","d":null,"cid":"1","c":[]}]},{"id":null,"n":"Event Hubs","d":null,"cid":"1","c":[{"id":null,"n":"Demos","d":null,"cid":"1","c":[]}]}]},{"id":null,"n":"Data & Analytics","d":null,"cid":"1","c":[{"id":null,"n":"SQL Database","d":null,"cid":"1","c":[{"id":null,"n":"Performance tuning","d":null,"cid":"1","c":[]},{"id":null,"n":"Comparison SQL Server","d":null,"cid":"1","c":[]},{"id":null,"n":"Tooling","d":null,"cid":"1","c":[]}]},{"id":null,"n":"HDInsight","d":null,"cid":"1","c":[]},{"id":null,"n":"Cache","d":null,"cid":"1","c":[]},{"id":null,"n":"Machine Learning","d":null,"cid":"1","c":[]},{"id":null,"n":"DocumentDB","d":null,"cid":"1","c":[]},{"id":null,"n":"Azure Search","d":null,"cid":"1","c":[]}]},{"id":null,"n":"Storage & Backup","d":"","cid":"1","c":[{"id":null,"n":"Storage","d":null,"cid":"1","c":[]},{"id":null,"n":"Import/Export Service","d":null,"cid":"1","c":[]},{"id":null,"n":"Backup","d":null,"cid":"1","c":[]},{"id":null,"n":"Site Recovery","d":null,"cid":"1","c":[]}]},{"id":null,"n":"Hybrid Integration","d":null,"cid":"1","c":[{"id":null,"n":"BizTalk Services","d":null,"cid":"1","c":[]},{"id":null,"n":"Service Bus","d":null,"cid":"1","c":[{"id":null,"n":"Demos","d":null,"cid":"1","c":[]}]}]},{"id":null,"n":"Identity & Access Management","d":"Accounts, Authentication, AAD etc.","cid":"1","c":[{"id":null,"n":"Azure Active Directory","d":null,"cid":"1","c":[{"id":null,"n":"Demos","d":null,"cid":"1","c":[]}]},{"id":null,"n":"Multi-Factor Authentication","d":null,"cid":"1","c":[]},{"id":null,"n":"Accounts","d":null,"cid":"6344b2c4-16dc-4500-910c-d36ffe09f121","c":[]}]},{"id":null,"n":"Media & CDN","d":null,"cid":"1","c":[{"id":null,"n":"Media Services","d":null,"cid":"1","c":[{"id":null,"n":"Demos","d":null,"cid":"1","c":[]}]},{"id":null,"n":"CDN","d":null,"cid":"1","c":[]}]}]},{"id":null,"n":"Network","d":"MS, MVP & Other","cid":"2","c":[{"id":null,"n":"WAZUGNL","d":null,"cid":"1","c":[{"id":null,"n":"Coming Event","d":null,"cid":"1","c":[]},{"id":null,"n":"Speaker pipeline","d":null,"cid":"1","c":[]}]}]},{"id":null,"n":"Guidance","d":"Decisions vs options","cid":"3","c":[{"id":null,"n":"CAT Practices","d":null,"cid":"1","c":[]},{"id":null,"n":"Community Practices","d":null,"cid":"1","c":[]},{"id":null,"n":"Tooling","d":null,"cid":"1","c":[{"id":null,"n":"Azure Explorer","d":null,"cid":"1","c":[]},{"id":null,"n":"CloudBerry Explorer","d":null,"cid":"1","c":[]},{"id":null,"n":"ServiceBus Manager","d":null,"cid":"1","c":[]},{"id":null,"n":null,"d":null,"cid":"1","c":[]}]}]},{"id":null,"n":"Development","d":"Skills & reference","cid":"1","c":[{"id":null,"n":".NET","d":null,"cid":"1","c":[{"id":null,"n":"Async","d":null,"cid":"1","c":[]},{"id":null,"n":"IOC","d":null,"cid":"1","c":[]},{"id":null,"n":"Reactive","d":null,"cid":"1","c":[]},{"id":null,"n":"Orleans","d":null,"cid":"1","c":[]}]},{"id":null,"n":"Javascript / HTML5","d":null,"cid":"1","c":[]},{"id":null,"n":"Other","d":null,"cid":"1","c":[]}]},{"id":null,"n":"R&D","d":"Looking into...xx","cid":"7fcbeddb-1064-48c3-8c36-db05eaf79656","c":[{"id":null,"n":"ADFS","d":"Details, best practices","cid":"1","c":[]},{"id":null,"n":"HDInsight","d":"Demo, overview talk","cid":"1","c":[]},{"id":null,"n":"SQL Database","d":"Performance tuning, best practices, comparison NoSQL and RDB alternatives","cid":"1","c":[]},{"id":null,"n":"Meteor","d":null,"cid":"1","c":[]}]}]}
            //mindMaps.Add(new MindMap
            //{
            //    Content = "{\"id\":null,\"n\":\"aap\",\"d\":null,\"cid\":null,\"c\":[{\"id\":null,\"n\":\"Azure Features\",\"d\":\"A to Z featureset\",\"cid\":\"ad5372b4-0602-4fbd-b0a6-4c576755a473\",\"c\":[{\"id\":null,\"n\":\"Compute & Networking\",\"d\":\"\",\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Virtual Machines\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Demos\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Links and Documents\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Pricing\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Azure RemoteApp\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Demos\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Cloud Services\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Demos\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Virtual Networks\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Demos\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"ExpressRoute\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Demos\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Traffic Manager\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Web & Mobile\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Websites\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Best practices\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Demos\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Techniques\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Mobile Services\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Techniques\",\"d\":\"\",\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Using Azure Storage with Mobile Services\",\"d\":\"http://chrisrisner.com/Mobile-Services-and-Windows-Azure-Storage\",\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Best practices\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Demos\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"API Management\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Demos\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Best practices\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Notification Hubs\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Demos\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Event Hubs\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Demos\",\"d\":null,\"cid\":\"1\",\"c\":[]}]}]},{\"id\":null,\"n\":\"Data & Analytics\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"SQL Database\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Performance tuning\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Comparison SQL Server\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Tooling\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"HDInsight\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Cache\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Machine Learning\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"DocumentDB\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Azure Search\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Storage & Backup\",\"d\":\"\",\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Storage\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Import/Export Service\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Backup\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Site Recovery\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Hybrid Integration\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"BizTalk Services\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Service Bus\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Demos\",\"d\":null,\"cid\":\"1\",\"c\":[]}]}]},{\"id\":null,\"n\":\"Identity & Access Management\",\"d\":\"Accounts, Authentication, AAD etc.\",\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Azure Active Directory\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Demos\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Multi-Factor Authentication\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Accounts\",\"d\":null,\"cid\":\"6344b2c4-16dc-4500-910c-d36ffe09f121\",\"c\":[]}]},{\"id\":null,\"n\":\"Media & CDN\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Media Services\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Demos\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"CDN\",\"d\":null,\"cid\":\"1\",\"c\":[]}]}]},{\"id\":null,\"n\":\"Network\",\"d\":\"MS, MVP & Other\",\"cid\":\"2\",\"c\":[{\"id\":null,\"n\":\"WAZUGNL\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Coming Event\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Speaker pipeline\",\"d\":null,\"cid\":\"1\",\"c\":[]}]}]},{\"id\":null,\"n\":\"Guidance\",\"d\":\"Decisions vs options\",\"cid\":\"3\",\"c\":[{\"id\":null,\"n\":\"CAT Practices\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Community Practices\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Tooling\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Azure Explorer\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"CloudBerry Explorer\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"ServiceBus Manager\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":null,\"d\":null,\"cid\":\"1\",\"c\":[]}]}]},{\"id\":null,\"n\":\"Development\",\"d\":\"Skills & reference\",\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\".NET\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Async\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"IOC\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Reactive\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Orleans\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Javascript / HTML5\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Other\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"R&D\",\"d\":\"Looking into...xx\",\"cid\":\"7fcbeddb-1064-48c3-8c36-db05eaf79656\",\"c\":[{\"id\":null,\"n\":\"ADFS\",\"d\":\"Details, best practices\",\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"HDInsight\",\"d\":\"Demo, overview talk\",\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"SQL Database\",\"d\":\"Performance tuning, best practices, comparison NoSQL and RDB alternatives\",\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Meteor\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Windows Ecosystem\",\"d\":\"Desktop,Slate & Phone\",\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Windows 8.1\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Windows Phone\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Other Microsoft Products\",\"d\":\"Servers & SaaS\",\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"CRM Dynamics\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Office 365\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Migration Plans\",\"d\":null,\"cid\":\"1\",\"c\":[]}]}]},{\"id\":null,\"n\":\"ToDo's\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"WAZUG Non-profit aanvraag\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"DemoButler\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"30-Day plan\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Trello integration\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Twitter feed\",\"d\":\"\",\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"O365 SDK REST\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Reminders\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Navragen aanbreng bonus Dennis\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"O365 Training\",\"d\":\"http://channel9.msdn.com/Series/Managing-Office-365-Identities-and-Services/10\",\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Notes\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":null,\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Quick Nodes\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Recordings\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Nodes\",\"d\":null,\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Blog\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"Posts\",\"d\":null,\"cid\":\"1\",\"c\":[]},{\"id\":null,\"n\":\"Statistics\",\"d\":\"Comments, views, IO\",\"cid\":\"1\",\"c\":[]}]},{\"id\":null,\"n\":\"Links\",\"d\":null,\"cid\":\"1\",\"c\":[{\"id\":null,\"n\":\"WAZUG OneNote\",\"d\":\"https://erwyn.sharepoint.com/teams/wazugnl/_layouts/15/WopiFrame.aspx?sourcedoc={6E45A30C-4AEC-4AA1-9B0D-AD97B39E15E9}&file=WAZUG%20NL%20Bestuur%20Notebook&action=default\",\"cid\":\"1\",\"c\":[]}]}]}",
            //    Id = "1",
            //    Name = "MyKnowledgeSoFar"

            //});

            //return mindMaps;
        }

        public async Task SaveMindMap(INode rootNode, IMindMap mindMap)
        {
            JsonNode jsonNode = new JsonNode();
            JsonNode jsonObject = BuildJSON(jsonNode, rootNode);

            mindMap.Content = JsonConvert.SerializeObject(jsonObject);
            var serializedGraph = JsonConvert.SerializeObject(mindMap);

            if (IsConnected())
            {
                CloudBlobContainer container = new CloudBlobContainer(new Uri(Settings.TenantGraphContainerSaS));
                CloudBlockBlob blob = container.GetBlockBlobReference(mindMap.Id);


                await blob.UploadTextAsync(serializedGraph);
            }

            StorageFolder localFolder =
                      ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await localFolder.CreateFileAsync(mindMap.Id, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(storageFile, serializedGraph);
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


        public async Task<IMindMap> CreateGraph(string graphName)
        {
            StorageFolder localFolder =
                    ApplicationData.Current.LocalFolder;

            Node rootNode = new Node();

            rootNode.Title = graphName;
            rootNode.Description = "[Description]";

            JsonNode jsonNode = new JsonNode();
            JsonNode jsonObject = BuildJSON(jsonNode, rootNode);

            var newGraph = new MindMap
            {
                Content = JsonConvert.SerializeObject(jsonObject),
                Id = Guid.NewGuid().ToString(),
                Name = graphName
            };

            StorageFile storageFile = await localFolder.CreateFileAsync(newGraph.Id, CreationCollisionOption.ReplaceExisting);

            string serializedGraph = JsonConvert.SerializeObject(newGraph);

            await FileIO.WriteTextAsync(storageFile, serializedGraph);

            if (IsConnected())
            {
                CloudBlobContainer container = new CloudBlobContainer(new Uri(Settings.TenantGraphContainerSaS));
                CloudBlockBlob blob = container.GetBlockBlobReference(newGraph.Id);
                blob.Metadata.Add("Name", graphName);
               
                await blob.UploadTextAsync(serializedGraph);
                await blob.SetMetadataAsync();
            }

            return newGraph;
        }

        public async Task<IMindMap> GetMindMap(string mindMapName)
        {
            string retrievedJson = null;

            try
            {
                //check cache first
                StorageFolder localFolder =
                    ApplicationData.Current.LocalFolder;
                if (await StorageHelper.DoesFileExistAsync
                    (mindMapName, localFolder))
                {
                    //use cached version
                    StorageFile file =
                        await localFolder.GetFileAsync(mindMapName);

                    if (IsConnected())
                    {
                        // BasicProperties props = await file.GetBasicPropertiesAsync();
                        string dateAccessedProperty = "System.DateModified";

                        List<string> propertiesName = new List<string>();
                        propertiesName.Add(dateAccessedProperty);

                        var dateFileChanged = await file.Properties.RetrievePropertiesAsync(propertiesName);


                        DateTime dt = DateTime.Parse(dateFileChanged.First().Value.ToString());

                        CloudBlobContainer container = new CloudBlobContainer(new Uri(Settings.TenantGraphContainerSaS));
                        bool existsInCloud = await container.GetBlockBlobReference(mindMapName).ExistsAsync();
                        if (existsInCloud)
                        {
                            CloudBlockBlob blob = container.GetBlockBlobReference(mindMapName);
                            await blob.FetchAttributesAsync();
                            if (dt.ToUniversalTime() < blob.Properties.LastModified)
                            {
                                //TODO:
                                //We have a newer file in the cloud. Handle it!
                                blob = container.GetBlockBlobReference(mindMapName);
                                retrievedJson = await blob.DownloadTextAsync();

                                //overwrite local
                                StorageFile storageFile = await localFolder.CreateFileAsync(mindMapName, CreationCollisionOption.ReplaceExisting);
                                await FileIO.WriteTextAsync(storageFile, retrievedJson);
                            }
                            else
                            {
                                //local file is newer
                                retrievedJson = await FileIO.ReadTextAsync(file);
                                await blob.UploadTextAsync(retrievedJson);
                            }
                        }
                        else
                        {
                            retrievedJson = await FileIO.ReadTextAsync(file);
                        }

                    }
                    else
                    {
                        retrievedJson = await FileIO.ReadTextAsync(file);
                    }
                }
                else //download and store now
                {
                    if (IsConnected())
                    {
                        CloudBlobContainer container = new CloudBlobContainer(new Uri(Settings.TenantGraphContainerSaS));
                        bool exists = await container.GetBlockBlobReference(mindMapName).ExistsAsync();


                        if (exists)
                        {
                            CloudBlockBlob blob = container.GetBlockBlobReference(mindMapName);
                            retrievedJson = await blob.DownloadTextAsync();
                        }

                        //store the response now
                        if (retrievedJson != null && retrievedJson != "")
                        {
                            StorageFile storageFile = await localFolder.CreateFileAsync(mindMapName, CreationCollisionOption.ReplaceExisting);
                            await FileIO.WriteTextAsync(storageFile, retrievedJson);
                        }
                    }
                    else
                    {
                        return new MindMap
                        {
                            Content = null,
                            Id = mindMapName,
                            Name = mindMapName
                        };
                    }
                }

                dynamic json = await Task.Run(() => JValue.Parse(retrievedJson));

                //return json;

                return new MindMap
                {
                    Content = json.content,
                    Id = json.Id,
                    Name = json.name
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
