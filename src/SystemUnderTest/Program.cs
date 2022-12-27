using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection.Metadata;
using SystemUnderTest;
using static System.Reflection.Metadata.BlobBuilder;


using IHost host = Host
    .CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
    })
.Build();



await UploadFileToAzBlob(host.Services);
await host.RunAsync();


static async Task UploadFileToAzBlob(IServiceProvider services)
{
    var config = services.GetRequiredService<IConfiguration>();
    var azBlobConnectionString = config.GetValue<string>("AzBlobSettings:ConnectionString");
    var azBlobContainerName = config.GetValue<string>("AzBlobSettings:ContainerName");
    var fileName = config.GetValue<string>("AzBlobSettings:FileName")!;

    using FileStream stream = new(fileName, FileMode.Open);
    
    BlobContainerClient container = new (azBlobConnectionString, azBlobContainerName);

    await container.CreateIfNotExistsAsync();

    try
    {
        var client = container.GetBlobClient(fileName);
        await client.UploadAsync(stream);
    }
    catch (Exception)
    {
        throw;
    }
}
