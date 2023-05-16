using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using SystemUnderTest.Interface;

namespace SystemUnderTest
{
    public static class Program
    {
        [ExcludeFromCodeCoverage]
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var azBlobService = host.Services.GetRequiredService<IAzBlobService>();
            await UploadFileToAzBlobAsync(azBlobService);

            await host.RunAsync();
        }
        [ExcludeFromCodeCoverage]
        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
           .CreateDefaultBuilder(args)
           .ConfigureServices((context, services) =>
           {
               var azBlobSettingsOption = context.Configuration.GetSection(AzBlobSettingsOption.ConfigKey).Get<AzBlobSettingsOption>()!;

               services.AddAzureClients(builder => builder
                                                    .AddBlobServiceClient(azBlobSettingsOption.ConnectionString)
                                                    .WithName(azBlobSettingsOption.ConnectionName));

               services.AddSingleton(azBlobSettingsOption);
               services.AddSingleton<IAzBlobService, AzBlobService>();
           })
           .ConfigureAppConfiguration((context, config) =>
           {
               config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
           });
        }

        public static async Task UploadFileToAzBlobAsync(IAzBlobService azBlobService)
        {
            var result = await azBlobService.UploadFileToAzBlobAsync("samplefile.txt");

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