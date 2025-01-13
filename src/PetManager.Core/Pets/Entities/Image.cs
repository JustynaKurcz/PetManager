namespace PetManager.Core.Pets.Entities;

public class Image
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string BlobUrl { get; set; }
    public DateTimeOffset UploadedAt { get; set; } = DateTimeOffset.UtcNow;
    public Guid PetId { get; set; }
    public Pet Pet { get; set; }

    private Image()
    {
    }

    private Image(string fileName, string blobUrl, Guid petId)
    {
        Id = Guid.NewGuid();
        FileName = fileName;
        BlobUrl = blobUrl;
        PetId = petId;
    }

    public static Image Create(string fileName, string blobUrl, Guid petId)
        => new(fileName, blobUrl, petId);
}