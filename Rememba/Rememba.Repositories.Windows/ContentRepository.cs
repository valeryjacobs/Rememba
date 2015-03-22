﻿using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Rememba.Contracts.Models;
using Rememba.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Storage;

namespace Rememba.Repositories.Windows
{
    public class ContentRepository
    {
        public static bool IsConnected()
        {
            return false;
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;

           
        }

        public async Task<ObservableCollection<IContent>> GetContentItems()
        {
            var contentItems = new ObservableCollection<IContent>();

            contentItems.Add(new Content
            {
                Id = Guid.NewGuid().ToString(),
                Data = "#Imported markdown\nFancy, huh?\n\n* Milk\n* Eggs\n* Salmon\n* Butter"
            });

            return contentItems;
        }

        public async Task<IContent> GetContent(string contentId)
        {
            if (contentId == null || contentId == "") contentId = Guid.NewGuid().ToString();

            string data = null;

            StorageFolder localFolder =
                  ApplicationData.Current.LocalFolder;
            if (await StorageHelper.DoesFileExistAsync
                (contentId, localFolder))
            {
                //use cached version
                StorageFile file =
                    await localFolder.GetFileAsync(contentId);
                data = await FileIO.ReadTextAsync(file);

                return new Content
                {
                    Id = contentId,
                    Data = data
                };
            }
            else //download and store now
            {
                if (IsConnected())
                {
                    CloudBlobContainer container = new CloudBlobContainer(new Uri(Settings.TenantContentContainerSaS));

                    bool exists = await container.GetBlockBlobReference(contentId).ExistsAsync();

                    if (exists)
                    {
                        CloudBlockBlob blob = container.GetBlockBlobReference(contentId);
                        data = await blob.DownloadTextAsync();
                    }

                    StorageFile storageFile = await localFolder.CreateFileAsync(contentId, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(storageFile, data);

                    return new Content
                    {
                        Id = contentId,
                        Data = data
                    };
                }
                else
                {
                    return new Content
                    {
                        Id = contentId,
                        Data = data
                    };
                }
            }
        }

        public async Task AddContent(IContent content)
        {
            if (content.Data == null) return;

            if (IsConnected())
            {

                CloudBlobContainer container = new CloudBlobContainer(new Uri(Settings.TenantContentContainerSaS));
                CloudBlockBlob blob = container.GetBlockBlobReference(content.Id);

                await blob.UploadTextAsync(content.Data);

                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile storageFile = await localFolder.CreateFileAsync(content.Id, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(storageFile, content.Data);
            }
            else
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile storageFile = await localFolder.CreateFileAsync(content.Id, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(storageFile, content.Data);
            }
        }

        public async Task UpdateContent(IContent content)
        {
            if (content.Data == null) return;

            if (content.Id == "1" || content.Id == "2")
            {
                content.Id = Guid.NewGuid().ToString();
                await AddContent(content);
            }
            else
            {
                if (IsConnected())
                {
                    CloudBlobContainer container = new CloudBlobContainer(new Uri(Settings.TenantContentContainerSaS));
                    CloudBlockBlob blob = container.GetBlockBlobReference(content.Id);

                    await blob.UploadTextAsync(content.Data);

                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    StorageFile storageFile = await localFolder.CreateFileAsync(content.Id, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(storageFile, content.Data);
                }
                else
                {

                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    StorageFile storageFile = await localFolder.CreateFileAsync(content.Id, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(storageFile, content.Data);
                }
              }
        }

        public async Task DeleteContent(string id)
        {
            CloudBlobContainer container = new CloudBlobContainer(new Uri(Settings.TenantContentContainerSaS));

            CloudBlockBlob blob = container.GetBlockBlobReference(id);

            //TODO:Instead of delete, throw it in a trashbin with a name extension so we can do some 'undoing'.
            await blob.DeleteAsync();
        }

        public async Task<string> DownloadContent(string contentId)
        {
            string stemp = "";

            if (IsConnected())
            {
                CloudBlobContainer container = new CloudBlobContainer(new Uri(Settings.TenantContentContainerSaS));
                CloudBlockBlob blob = container.GetBlockBlobReference(contentId);

                // Get reference to the file in blob storage
                CloudBlockBlob blobFromSASCredential = container.GetBlockBlobReference(contentId);

                stemp = await blobFromSASCredential.DownloadTextAsync();

                //store the JSON file from Blob storage to Windows local Storage
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile file = await folder.CreateFileAsync(contentId,
                    CreationCollisionOption.ReplaceExisting);
                Stream outStream = await file.OpenStreamForWriteAsync();

                await blobFromSASCredential.DownloadToStreamAsync(outStream.AsOutputStream());
            }


            return stemp;
        }
    }
}
