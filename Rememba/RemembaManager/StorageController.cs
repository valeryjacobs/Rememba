using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemembaManager
{
    public static class StorageController
    {
        private const string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=rememba;AccountKey=XNJWMO3RMi+58l4QDonBju98zVDyBVdk/fcXTEVNFJzlUx0f7766B2KbpUXHIkDlrIcPmqbWwLSVlWHyCCX7Ng==";
        public static string GenerateKey(string tenantId)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

           
            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(tenantId);
            container.CreateIfNotExists();

            CreateSharedAccessPolicy(blobClient, container, tenantId);

            

            return GetContainerSasUriWithPolicy(container, tenantId);
        }
        static string GetContainerSasUri(CloudBlobContainer container)
        {
            //Set the expiry time and permissions for the container.
            //In this case no start time is specified, so the shared access signature becomes valid immediately.
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddYears(1);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List;

            //Generate the shared access signature on the container, setting the constraints directly on the signature.
            string sasContainerToken = container.GetSharedAccessSignature(sasConstraints);

            //Return the URI string for the container, including the SAS token.
            return container.Uri + sasContainerToken;
        }

        public static void CreateSharedAccessPolicy(CloudBlobClient blobClient, CloudBlobContainer container, string policyName)
        {
            //Create a new stored access policy and define its constraints.
            SharedAccessBlobPolicy sharedPolicy = new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddYears(1),
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List
            };

            //Get the container's existing permissions.
            BlobContainerPermissions permissions = new BlobContainerPermissions();

            //Add the new policy to the container's permissions.
            permissions.SharedAccessPolicies.Clear();
            permissions.SharedAccessPolicies.Add(policyName, sharedPolicy);
            container.SetPermissions(permissions);
        }

        public static string GetContainerSasUriWithPolicy(CloudBlobContainer container, string policyName)
        {
            //Generate the shared access signature on the container. In this case, all of the constraints for the 
            //shared access signature are specified on the stored access policy.
            string sasContainerToken = container.GetSharedAccessSignature(null, policyName);

            //Return the URI string for the container, including the SAS token.
            return container.Uri + sasContainerToken;
        }

        public static string GetBlobSasUriWithPolicy(CloudBlobContainer container, string policyName,string blobReference)
        {
            //Get a reference to a blob within the container.
            CloudBlockBlob blob = container.GetBlockBlobReference(blobReference);

            //Upload text to the blob. If the blob does not yet exist, it will be created. 
            //If the blob does exist, its existing content will be overwritten.
            string blobContent = "This blob will be accessible to clients via a shared access signature. " +
            "A stored access policy defines the constraints for the signature.";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(blobContent));
            ms.Position = 0;
            using (ms)
            {
                blob.UploadFromStream(ms);
            }

            //Generate the shared access signature on the blob.
            string sasBlobToken = blob.GetSharedAccessSignature(null, policyName);

            //Return the URI string for the container, including the SAS token.
            return blob.Uri + sasBlobToken;
        }

    }
}
