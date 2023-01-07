using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SystemUnderTest.Interface;

using IHost host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IAzBlobService, AzBlobService>();
    })
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
    var azBlobService = services.GetRequiredService<IAzBlobService>();
    var result = await azBlobService.UploadFileToAzBlob("samplefile.txt");

    if (result)
    {
        Console.WriteLine("File uploaded successfully");
    }
    else
    {
        Console.WriteLine("File upload failed");
    }
}
