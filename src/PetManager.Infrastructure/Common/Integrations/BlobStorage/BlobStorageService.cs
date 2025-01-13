using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using PetManager.Application.Common.Integrations.BlobStorage;
using PetManager.Infrastructure.Common.Integrations.BlobStorage.Options;

namespace PetManager.Infrastructure.Common.Integrations.BlobStorage;

public class BlobStorageService(AzureBlobStorageOptions options) : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient = new(options.ConnectionString);

    public async Task<string> UploadImageAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var account = CloudStorageAccount.Parse(options.ConnectionString);
        var client = account.CreateCloudBlobClient();
        var container = client.GetContainerReference(options.ContainerName);
        await container.CreateIfNotExistsAsync(cancellationToken);
        var photo = container.GetBlockBlobReference(Path.GetFileName(file.FileName));
        await using var stream = file.OpenReadStream();
        await photo.UploadFromStreamAsync(stream, cancellationToken);

        return photo.Uri.ToString();
    }

    public async Task DeleteImageAsync(string blobUrl, CancellationToken cancellationToken)
    {
        var uri = new Uri(blobUrl);
        var blobName = Path.GetFileName(uri.LocalPath);

        var containerClient = _blobServiceClient.GetBlobContainerClient(options.ContainerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }
}