using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Azure;
using Moq;
using SystemUnderTest.Interface;

namespace SystemUnderTest.UnitTest;

public class AzBlobServiceTest
{
    private readonly StringWriter Output = new();
    private const string _connectionName = "testconnection";

    public AzBlobServiceTest()
    {
        Console.SetOut(Output);
    }
    [Fact]
    public async Task File_Upload_Suceess()
    {
    // Arrange
        var _azBlobServiceClientFactory = new Mock<IAzureClientFactory<BlobServiceClient>>();
        var _azBlobServiceClient = new Mock<BlobServiceClient>();
        var _azBlobContainerClient = new Mock<BlobContainerClient>();
        var _azBlobClient = new Mock<BlobClient>();

        var blobContainerInfo = BlobsModelFactory.BlobContainerInfo(default,default);
        var blobContentInfo = BlobsModelFactory.BlobContentInfo(default,default,default,default,default);

        _azBlobContainerClient.Setup(x => x.CreateIfNotExistsAsync(default,default,default,default)).ReturnsAsync(Response.FromValue(blobContainerInfo,default!));
        _azBlobClient.Setup(x => x.UploadAsync(It.IsAny<Stream>(), default, default, default, default,default,default,default)).ReturnsAsync(Response.FromValue(blobContentInfo,default!));
        _azBlobContainerClient.Setup(x => x.GetBlobClient(It.IsAny<string>())).Returns(_azBlobClient.Object);
        _azBlobServiceClient.Setup(x => x.GetBlobContainerClient(It.IsAny<string>())).Returns(_azBlobContainerClient.Object);

        _azBlobServiceClientFactory.Setup(x => x.CreateClient(_connectionName)).Returns(_azBlobServiceClient.Object);
        var _azBlobSettingsOption = new AzBlobSettingsOption()
        {
            ConnectionName = _connectionName,
        };
        var _sut = new AzBlobService(_azBlobServiceClientFactory.Object, _azBlobSettingsOption);

    // Act
        var result = await _sut.UploadFileToAzBlobAsync("samplefile.txt");

    // Assert
        Assert.True(result);
    }

     [Fact]
    public async Task File_Upload_Fail()
    {
    // Arrange
        var _azBlobServiceClientFactory = new Mock<IAzureClientFactory<BlobServiceClient>>();
        var _azBlobServiceClient = new Mock<BlobServiceClient>();
        var _azBlobContainerClient = new Mock<BlobContainerClient>();
        var _azBlobClient = new Mock<BlobClient>();

        var blobContainerInfo = BlobsModelFactory.BlobContainerInfo(default,default);
        var blobContentInfo = BlobsModelFactory.BlobContentInfo(default,default,default,default,default);

        _azBlobContainerClient.Setup(x => x.CreateIfNotExistsAsync(default,default,default,default)).ReturnsAsync(Response.FromValue(blobContainerInfo,default!));
        _azBlobClient.Setup(x => x.UploadAsync(It.IsAny<Stream>(), default, default, default, default,default,default,default)).ReturnsAsync(Response.FromValue(blobContentInfo,default!));
        _azBlobContainerClient.Setup(x => x.GetBlobClient(It.IsAny<string>())).Returns(_azBlobClient.Object);
        _azBlobServiceClient.Setup(x => x.GetBlobContainerClient(It.IsAny<string>())).Returns(_azBlobContainerClient.Object);

        _azBlobServiceClientFactory.Setup(x => x.CreateClient(_connectionName)).Returns(_azBlobServiceClient.Object);
        var _azBlobSettingsOption = new AzBlobSettingsOption()
        {
            ConnectionName = _connectionName,
        };
        var _sut = new AzBlobService(_azBlobServiceClientFactory.Object, _azBlobSettingsOption);

    // Act
        var result = await _sut.UploadFileToAzBlobAsync("");

    // Assert
        Assert.False(result);
        Assert.Contains("Unable to upload blob. Reason", Output.ToString());
    }
}
