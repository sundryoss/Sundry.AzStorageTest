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
        public async Task File_Upload_Suceess()
        {
            await Program.UploadFileToAzBlobAsync(_azBlobService);
            Assert.Contains("File uploaded successfully", Output.ToString());
        }
    }
}
