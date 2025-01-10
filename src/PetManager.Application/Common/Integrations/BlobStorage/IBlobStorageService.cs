using Microsoft.AspNetCore.Http;

namespace PetManager.Application.Common.Integrations.BlobStorage;

public interface IBlobStorageService
{
    Task<string> UploadImageAsync(IFormFile file, CancellationToken cancellationToken);
    Task DeleteImageAsync(string blobUrl, CancellationToken cancellationToken);
}