using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FakeItEasy;
using Microsoft.Extensions.Azure;
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
        var _azBlobServiceClientFactory = A.Fake<IAzureClientFactory<BlobServiceClient>>();
        var _azBlobServiceClient = A.Fake<BlobServiceClient>();
        var _azBlobContainerClient = A.Fake<BlobContainerClient>();
        var _azBlobClient = A.Fake<BlobClient>();
        var _azBlobSettingsOption = A.Fake<AzBlobSettingsOption>();


        var blobContainerInfo = BlobsModelFactory.BlobContainerInfo(default, default);
        var blobContentInfo = BlobsModelFactory.BlobContentInfo(default, default, default, default, default);

        A.CallTo(() => _azBlobContainerClient.CreateIfNotExistsAsync(default, default, default, default)).Returns(Response.FromValue(blobContainerInfo, default!));
        A.CallTo(() => _azBlobClient.UploadAsync(A.Dummy<Stream>(), default, default, default, default, default, default, default)).Returns(Response.FromValue(blobContentInfo, default!));
        A.CallTo(() => _azBlobContainerClient.GetBlobClient(default)).Returns(_azBlobClient);
        A.CallTo(() => _azBlobServiceClient.GetBlobContainerClient(default)).Returns(_azBlobContainerClient);
        A.CallTo(() => _azBlobServiceClientFactory.CreateClient(default)).Returns(_azBlobServiceClient);



        var _sut = new AzBlobService(_azBlobServiceClientFactory, _azBlobSettingsOption);

        // Act
        var result = await _sut.UploadFileToAzBlobAsync("samplefile.txt");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task File_Upload_Fail()
    {
       // Arrange
        var _azBlobServiceClientFactory = A.Fake<IAzureClientFactory<BlobServiceClient>>();
        var _azBlobServiceClient = A.Fake<BlobServiceClient>();
        var _azBlobContainerClient = A.Fake<BlobContainerClient>();
        var _azBlobClient = A.Fake<BlobClient>();
        var _azBlobSettingsOption = A.Fake<AzBlobSettingsOption>();


        var blobContainerInfo = BlobsModelFactory.BlobContainerInfo(default, default);
        var blobContentInfo = BlobsModelFactory.BlobContentInfo(default, default, default, default, default);

        A.CallTo(() => _azBlobContainerClient.CreateIfNotExistsAsync(default, default, default, default)).Returns(Response.FromValue(blobContainerInfo, default!));
        A.CallTo(() => _azBlobClient.UploadAsync(A.Dummy<Stream>(), default, default, default, default, default, default, default)).Returns(Response.FromValue(blobContentInfo, default!));
        A.CallTo(() => _azBlobContainerClient.GetBlobClient(default)).Returns(_azBlobClient);
        A.CallTo(() => _azBlobServiceClient.GetBlobContainerClient(default)).Returns(_azBlobContainerClient);
        A.CallTo(() => _azBlobServiceClientFactory.CreateClient(default)).Returns(_azBlobServiceClient);

        var _sut = new AzBlobService(_azBlobServiceClientFactory, _azBlobSettingsOption);

        // Act
        var result = await _sut.UploadFileToAzBlobAsync("");

        // Assert
        Assert.False(result);
        Assert.Contains("Unable to upload blob. Reason", Output.ToString());
    }
}
