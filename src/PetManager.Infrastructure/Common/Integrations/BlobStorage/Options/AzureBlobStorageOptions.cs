namespace PetManager.Infrastructure.Common.Integrations.BlobStorage.Options;

public record AzureBlobStorageOptions
{
    public string ConnectionString { get; init; }
    public string ContainerName { get; init; }
}