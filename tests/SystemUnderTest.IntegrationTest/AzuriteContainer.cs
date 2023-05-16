using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;

namespace SystemUnderTest.IntegrationTest
{
    public class AzuriteContainer : IAsyncLifetime
    {
        private readonly AzuriteTestcontainer _azuriteContainer;
        private const string AZURE_IMAGE = "mcr.microsoft.com/azure-storage/azurite";
        private const int DEFAULT_BLOB_PORT = 10000;
        public AzuriteContainer()
        {
            _azuriteContainer = new TestcontainersBuilder<AzuriteTestcontainer>()
                                .WithAzurite(new AzuriteTestcontainerConfiguration(AZURE_IMAGE)
                                {
                                    BlobServiceOnlyEnabled = true,
                                })
                                .WithPortBinding(DEFAULT_BLOB_PORT)
                                .Build();
        }
        public async Task DisposeAsync()
        {
            await _azuriteContainer.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            await _azuriteContainer.StartAsync(cts.Token);
        }
    }
}
