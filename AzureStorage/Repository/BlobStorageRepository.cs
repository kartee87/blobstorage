using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AzureStorage.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Globalization;

namespace AzureStorage.Repository
{
	public class BlobStorageRepository : IBlobStorageRepository
	{
		private StorageCredentials _storageCredentialsx;
		private CloudStorageAccount _cloudStorageAccountx;
		private CloudBlobClient _cloudBlobClientx;
		private CloudBlobContainer _cloudBlobContainerx;
		
		private String containerNamex = "sample-container";
        private String storageEndPointx = "core.windows.net";
        private String downloadPath = @"C:\";
		//string storageAccountName = "wpptestblob";
  //      string storageEndPoint = "core.windows.net";
		//string applicationId = "2801cb4a-8cf8-4a4d-8886-d51b0dbe375a";
  //      string clientSecret = "Wv6kKHJup4d3vR7WvtVrlE]GTKfIN=@=";
  //      string tenantId = "188285f7-8f1e-4c0d-a0bc-797e3e38c5b3";
		public BlobStorageRepository()
		{
			string accountNamex = "wppblob";
			string keyx = "KoA3oMIiUA0FST2XjbJweg0/kcxjJD1+8BJyAtH/nE8iVw6+OFVYAlgro3gVXxGRRz7e9GkURlRKFZA0XQtR2A==";
			_storageCredentialsx = new StorageCredentials(accountNamex, keyx);
			_cloudStorageAccountx = new CloudStorageAccount(_storageCredentialsx, true);
			_cloudBlobClientx = _cloudStorageAccountx.CreateCloudBlobClient();
			_cloudBlobContainerx = _cloudBlobClientx.GetContainerReference(containerNamex);

		}
		public bool DeleteBlob(string file, string fileExtension)
		{
            //throw new NotImplementedException();

            _cloudBlobContainerx = _cloudBlobClientx.GetContainerReference(containerNamex);

            //string accessToken = GetUserOAuthToken(tenantId, applicationId, clientSecret);
            //TokenCredential tokenCredential = new TokenCredential(accessToken);
            //StorageCredentials storageCredentials = new StorageCredentials(tokenCredential);
            //CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, storageAccountName, storageEndPoint, useHttps: true);
            //CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
            //CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerNamex);

            CloudBlockBlob blockBlob = _cloudBlobContainerx.GetBlockBlobReference(file + "." + fileExtension);
			bool deleted = blockBlob.DeleteIfExists();
			return deleted;
		}

		public async Task<bool> DownloadBlobAsync(string file, string fileExtension)
		{
            _cloudBlobContainerx = _cloudBlobClientx.GetContainerReference(containerNamex);
            CloudBlockBlob blockBlob = _cloudBlobContainerx.GetBlockBlobReference(file + fileExtension);
           
            //string accessToken = GetUserOAuthToken(tenantId, applicationId, clientSecret);
   //         TokenCredential tokenCredential = new TokenCredential(accessToken);
			//StorageCredentials storageCredentials = new StorageCredentials(tokenCredential);
			//CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, storageAccountName, storageEndPoint, useHttps: true);
			//CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
			//CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerNamex);

			//CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(file + fileExtension);


			using (var filestream = System.IO.File.OpenWrite(downloadPath + file + fileExtension))
			{
				await blockBlob.DownloadToStreamAsync(filestream);

				return true;
			}
		}

		public IEnumerable<BlobViewModel> GetBlobs()
		{

            var context = _cloudBlobContainerx.ListBlobs().ToList();

   //         string accessToken = GetUserOAuthToken(tenantId, applicationId, clientSecret);
			//TokenCredential tokenCredential = new TokenCredential(accessToken);
			//StorageCredentials storageCredentials = new StorageCredentials(tokenCredential);
			//CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, storageAccountName, storageEndPoint, useHttps: true);
			//CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
			//CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerNamex);

			//var context = blobContainer.ListBlobs().ToList();

			IEnumerable<BlobViewModel> VM = context.Select(x => new BlobViewModel
			{
				BlobContainerName = x.Container.Name,
				StorageUri = x.StorageUri.PrimaryUri.ToString(),
				PrimaryUri = x.StorageUri.PrimaryUri.ToString(),
				ActualFileName = x.Uri.AbsoluteUri.Substring(x.Uri.AbsolutePath.LastIndexOf("/") + 1),
				fileExtension = System.IO.Path.GetExtension(x.Uri.AbsoluteUri.Substring(x.Uri.AbsolutePath.LastIndexOf("/") + 1))
			}).ToList();
			return VM;
		}

		public bool UpdateBlob(HttpPostedFileBase blobFile)
		{
			//throw new NotImplementedException();
			if (blobFile == null)
			{
				return false;
			}

            _cloudBlobContainerx = _cloudBlobClientx.GetContainerReference(containerNamex);
            CloudBlockBlob blockBlob = _cloudBlobContainerx.GetBlockBlobReference(blobFile.FileName);
   //         string accessToken = GetUserOAuthToken(tenantId, applicationId, clientSecret);
			//TokenCredential tokenCredential = new TokenCredential(accessToken);
			//StorageCredentials storageCredentials = new StorageCredentials(tokenCredential);
			//CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, storageAccountName, storageEndPoint, useHttps: true);
			//CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
			//CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerNamex);

			//CloudBlockBlob blockBlobs = blobContainer.GetBlockBlobReference(blobFile.FileName);

			using (var fileStream = (blobFile.InputStream))
			{
				blockBlob.UploadFromStream(fileStream);
			}
			return true;
		}

		//static string GetUserOAuthToken(string tenantId, string applicationId, string clientSecret)
		//{
		//	const string ResourceId = "https://storage.azure.com/";
		//	const string AuthInstance = "https://login.microsoftonline.com/{0}/";
			
		//	string authority = string.Format(CultureInfo.InvariantCulture, AuthInstance, tenantId);
		//	AuthenticationContext authContext = new AuthenticationContext(authority);
		//	var clientCred = new ClientCredential(applicationId, clientSecret);
		//	AuthenticationResult result = authContext.AcquireTokenAsync(ResourceId, clientCred).Result;
		//	return result.AccessToken;
		//}
	}
}