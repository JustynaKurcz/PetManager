using PetManager.Application.Common.Integrations.BlobStorage;

namespace PetManager.Infrastructure.Common.Integrations.BlobStorage;

internal static class BlobStorageExtensions
{
    private const string AzureBlobStorageSectionName = "AzureBlobStorage";

    public static IServiceCollection AddBlobStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var azureBlobStorageOptions =
            configuration.GetSection(AzureBlobStorageSectionName).Get<AzureBlobStorageOptions>();
        services.AddSingleton(azureBlobStorageOptions);

        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        return services;
    }
}