using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SystemUnderTest.Interface;

namespace SystemUnderTest.IntegrationTest
{
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder) =>
            hostBuilder
             .ConfigureServices((context, services) =>
             {
                 services.AddSingleton<IAzBlobService, AzBlobService>();
             })
            .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
                });

    }
}
