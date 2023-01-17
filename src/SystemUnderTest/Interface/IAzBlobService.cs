using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace SystemUnderTest.Interface
{
    public interface IAzBlobService
    {
        Task<bool> UploadFileToAzBlobAsync(string fileName);
        Task<BlobDownloadResult> DownloadBlobAsync(string fileName);
    }

    public class AzBlobService : IAzBlobService
    {
        private readonly string azBlobConnectionString;
        private readonly string azBlobContainerName;
        public AzBlobService(IConfiguration configuration)
        {
            azBlobConnectionString = configuration.GetValue<string>("AzBlobSettings:ConnectionString")!;
            azBlobContainerName = configuration.GetValue<string>("AzBlobSettings:ContainerName")!;
        }
        public async Task<bool> UploadFileToAzBlobAsync(string fileName)
        {
            using FileStream stream = new(fileName, FileMode.Open);
            BlobContainerClient container = new(azBlobConnectionString, azBlobContainerName);

            try
            {
                await container.CreateIfNotExistsAsync();
                var client = container.GetBlobClient(fileName);
                await client.UploadAsync(stream);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to upload blob. Reason :{ex.Message}");
                return false;
            }
        }
        public async Task<BlobDownloadResult> DownloadBlobAsync(string fileName)
        {
            BlobClient blobClient = new(azBlobConnectionString, azBlobContainerName, fileName);
            try
            {
                BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
                return downloadResult;
            }
            catch
            {
                throw;
            }
        }
    }
}
