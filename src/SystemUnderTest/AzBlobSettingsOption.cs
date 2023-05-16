namespace SystemUnderTest;

public class AzBlobSettingsOption
{
    public const string ConfigKey = "AzBlobSettings";
    public string ConnectionName { get; set; } = default!;
    public string ConnectionString { get; set; } = default!;
    public string ContainerName { get; set; } = default!;
}
