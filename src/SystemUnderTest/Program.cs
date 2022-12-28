using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using SystemUnderTest.Interface;

namespace SystemUnderTest
{
    public class Program
    {
        [ExcludeFromCodeCoverage]
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var azBlobService = host.Services.GetRequiredService<IAzBlobService>();
            await UploadFileToAzBlob(azBlobService);

            await host.RunAsync();
        }
        [ExcludeFromCodeCoverage]
         static IHostBuilder CreateHostBuilder(string[] args) =>
           Host
            .CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IAzBlobService, AzBlobService>();
            })
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
            });

        public static async Task UploadFileToAzBlob(IAzBlobService azBlobService)
        {
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

    }
}





