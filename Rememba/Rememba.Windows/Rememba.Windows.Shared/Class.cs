using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Rememba.Windows
{
    class Class
    {
        private async Task<int> UploadToAzureStorage()
        {
            try
            {
                //  create Azure Storage
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=universalappazurestorage;AccountKey=<your key>");

                //  create a blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                //  create a container 
                CloudBlobContainer container = blobClient.GetContainerReference("containerone");

                //  create a block blob
                CloudBlockBlob blockBlob = container.GetBlockBlobReference("filename");

                //  create a local file
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("filename", CreationCollisionOption.ReplaceExisting);

                //  copy some txt to local file
                MemoryStream ms = new MemoryStream();
                DataContractSerializer serializer = new DataContractSerializer(typeof(string));
                serializer.WriteObject(ms, "Hello Azure World!!");

                using (Stream fileStream = await file.OpenStreamForWriteAsync())
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    await ms.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }

                //  upload to Azure Storage 
                await blockBlob.UploadFromFileAsync(file);

                return 1;
            }
            catch
            {
                //  return error
                return 0;
            }
        }
    }
}
