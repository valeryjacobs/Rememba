using Microsoft.WindowsAzure.Storage.Auth;
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
using Windows.Storage;

namespace Rememba.Repositories.Windows
{
    public class ContentRepository
    {
        private const string tenantContainerSaS = "https://rememba.blob.core.windows.net/34a3e707-00d8-4172-91a1-873ce6efd843?sv=2014-02-14&sr=c&si=34a3e707-00d8-4172-91a1-873ce6efd843&sig=BSLN7oWVtOTKgY9YE2dGXVtemmWRp99AGHqt2cLYoag%3D";


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

            CloudBlobContainer container = new CloudBlobContainer(new Uri(tenantContainerSaS));
            

            bool exists = await container.GetBlockBlobReference(contentId).ExistsAsync();
            var data = "";

            if (exists)
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(contentId);
                data = await blob.DownloadTextAsync();
            }

            return new Content
            {
                Id = contentId,
                Data = data
            };
        }

        public async Task AddContent(IContent content)
        {

            CloudBlobContainer container = new CloudBlobContainer(new Uri(tenantContainerSaS));
            CloudBlockBlob blob = container.GetBlockBlobReference(content.Id);

            await blob.UploadTextAsync(content.Data);
        }

        public async Task UpdateContent(IContent content)
        {
            CloudBlobContainer container = new CloudBlobContainer(new Uri(tenantContainerSaS));
            CloudBlockBlob blob = container.GetBlockBlobReference(content.Id);

            //StorageFolder folder = ApplicationData.Current.LocalFolder;
            //StorageFile fileToUpload = await folder.GetFileAsync(content.Id);
            //Stream uploadStream = await fileToUpload.OpenStreamForReadAsync();


            //await blob.UploadFromStreamAsync(uploadStream.AsInputStream());
            await blob.UploadTextAsync(content.Data);
        }

        public async Task DeleteContent(string id)
        {
            CloudBlobContainer container = new CloudBlobContainer(new Uri(tenantContainerSaS));

            CloudBlockBlob blob = container.GetBlockBlobReference(id);

            //TODO:Instead of delete, throw it in a trashbin with a name extension so we can do some 'undoing'.
            await blob.DeleteAsync();
        }

        public async Task<string> DownloadContent(string contentId)
        {
            CloudBlobContainer container = new CloudBlobContainer(new Uri(tenantContainerSaS));
            CloudBlockBlob blob = container.GetBlockBlobReference(contentId);

            // Get reference to the file in blob storage
            CloudBlockBlob blobFromSASCredential = container.GetBlockBlobReference(contentId);

            string stemp = await blobFromSASCredential.DownloadTextAsync();

            //store the JSON file from Blob storage to Windows local Storage
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync(contentId,
                CreationCollisionOption.ReplaceExisting);
            Stream outStream = await file.OpenStreamForWriteAsync();

            await blobFromSASCredential.DownloadToStreamAsync(outStream.AsOutputStream());

            return stemp;
        }

    }
}
