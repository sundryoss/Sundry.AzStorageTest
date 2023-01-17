using Azure;
using Azure.Storage.Blobs;
using SystemUnderTest.Interface;

namespace SystemUnderTest.IntegrationTest;

[Collection(nameof(AzuriteContainer))]
[TestCaseOrderer("SystemUnderTest.IntegrationTest.SequenceOrderer", "SystemUnderTest.IntegrationTest")]

public class IntegrationTests
{
    private readonly StringWriter Output = new();
    private readonly IAzBlobService _azBlobService;

    public IntegrationTests(IAzBlobService azBlobService, AzuriteContainer azuriteInstance)
    {
        Console.SetOut(Output);
       _azBlobService = azBlobService;
    }

    [Fact, TestSequence(0)]
    public async Task File_Upload_Success()
    {
        await Program.UploadFileToAzBlobAsync(_azBlobService);
        Assert.Contains("File uploaded successfully", Output.ToString());
    }

    [Fact, TestSequence(1)]
    public async Task File_Download_Success()
    {
        await Program.DownloadBlobAsync(_azBlobService);
        Assert.Contains("Downloaded data: This is sample file.", Output.ToString());
    }
}
