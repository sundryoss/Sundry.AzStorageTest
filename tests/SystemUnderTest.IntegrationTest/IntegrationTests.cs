using Azure;
using SystemUnderTest.Interface;

namespace SystemUnderTest.IntegrationTest
{
    [Collection(nameof(AzuriteContainer))]
    public class IntegrationTests
    {
        private readonly StringWriter Output = new();
        private readonly IAzBlobService _azBlobService;

        public IntegrationTests(IAzBlobService azBlobService, AzuriteContainer azuriteInstance)
        {
            Console.SetOut(Output);
           _azBlobService = azBlobService;
        }

        [Fact]
        public async Task File_Upload_Success()
        {
            await Program.UploadFileToAzBlobAsync(_azBlobService);
            Assert.Contains("File uploaded successfully", Output.ToString());
        }

        [Fact]
        public async Task File_Download_Success()
        {
            await Program.UploadFileToAzBlobAsync(_azBlobService);
            await Program.DownloadBlobAsync(_azBlobService);
            Assert.Contains("Downloaded data: This is sample file.", Output.ToString());
        }

        [Fact]
        public async Task File_Download_Failure()
        {
          await  Assert.ThrowsAsync<RequestFailedException>(() => Program.DownloadBlobAsync(_azBlobService));
        }
    }
}
