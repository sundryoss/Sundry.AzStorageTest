namespace SystemUnderTest.IntegrationTest
{
    [CollectionDefinition(nameof(AzuriteContainer))]
    public class AzuriteContainerInstanceCollectionFixture : ICollectionFixture<AzuriteContainer>
    {
    }
}
