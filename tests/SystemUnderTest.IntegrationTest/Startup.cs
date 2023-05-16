using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SystemUnderTest.Interface;
using Microsoft.Extensions.Azure;

namespace SystemUnderTest.IntegrationTest
{
    public static class Startup
    {
        public static void ConfigureHost(IHostBuilder hostBuilder) =>
            hostBuilder
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
}
