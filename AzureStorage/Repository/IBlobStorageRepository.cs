using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AzureStorage.Models;
using System.Threading.Tasks;

namespace AzureStorage.Repository
{
	public interface IBlobStorageRepository
	{
		IEnumerable<BlobViewModel> GetBlobs();
		bool DeleteBlob(string file, string fileExtension);
		bool UpdateBlob(HttpPostedFileBase blobFile);

		Task<bool> DownloadBlobAsync(string file, string fileExtension);
	}
}