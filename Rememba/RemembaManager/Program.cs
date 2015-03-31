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

            var tenantId = Guid.NewGuid().ToString();//"34a3e707-00d8-4172-91a1-873ce6efd843"
            var key = StorageController.GenerateKey(tenantId);

            //var key = "https://rememba.blob.core.windows.net/34a3e707-00d8-4172-91a1-873ce6efd843?sv=2014-02-14&sr=c&si=34a3e707-00d8-4172-91a1-873ce6efd843&sig=BSLN7oWVtOTKgY9YE2dGXVtemmWRp99AGHqt2cLYoag%3D";

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
