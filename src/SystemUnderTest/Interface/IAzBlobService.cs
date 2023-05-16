using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;

namespace SystemUnderTest.Interface
{
    public interface IAzBlobService
    {
        Task<bool> UploadFileToAzBlobAsync(string fileName);
    }

    public class AzBlobService : IAzBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly AzBlobSettingsOption _azBlobSettingsOption;
        public AzBlobService(IAzureClientFactory<BlobServiceClient> blobServiceClientFactory, AzBlobSettingsOption azBlobSettingsOption)
        {
            _azBlobSettingsOption = azBlobSettingsOption;
            _blobServiceClient = blobServiceClientFactory.CreateClient(_azBlobSettingsOption.ConnectionName);
        }
        public async Task<bool> UploadFileToAzBlobAsync(string fileName)
        {
            using FileStream stream = new(fileName, FileMode.Open);
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(_azBlobSettingsOption.ContainerName);

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
    }
}
