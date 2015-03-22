using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemembaManager
{
    class Program
    {
        static void Main(string[] args)
        {

            var tenantId = "34a3e707-00d8-4172-91a1-873ce6efd843graphs";// Guid.NewGuid().ToString();
            var key = StorageController.GenerateKey(tenantId);


            CloudBlobContainer container = new CloudBlobContainer(new Uri(key));
            CloudBlockBlob blob = container.GetBlockBlobReference("blobCreatedViaSAS.txt");
            string blobContent = "This blob was created with a shared access signature granting write permissions to the container. ";
            MemoryStream msWrite = new MemoryStream(Encoding.UTF8.GetBytes(blobContent));
            msWrite.Position = 0;
            using (msWrite)
            {
                blob.UploadFromStream(msWrite);
            }
            

        }
    }
}
