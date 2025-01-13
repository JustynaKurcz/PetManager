using System.Text;
using Microsoft.AspNetCore.Http;

namespace PetManager.Tests.Integration.Pets.Helpers;

public static class FormFileGenerator
{
    private const string DefaultMimeType = "image/jpeg";
    private const int InitialStreamOffset = 0;

    public static Faker<IFormFile> CreateTestFileFaker()
    {
        return new Faker<IFormFile>()
            .CustomInstantiator(faker => GenerateTestFormFile(
                filename: faker.System.FileName(),
                mimeType: DefaultMimeType,
                fileContent: faker.Lorem.Paragraph()
            ));
    }

    private static IFormFile GenerateTestFormFile(
        string filename,
        string mimeType,
        string fileContent)
    {
        var contentBytes = Encoding.UTF8.GetBytes(fileContent);
        var contentStream = new MemoryStream(contentBytes);

        var testFile = new FormFile(
            baseStream: contentStream,
            baseStreamOffset: InitialStreamOffset,
            length: contentBytes.Length,
            name: filename,
            fileName: filename)
        {
            Headers = new HeaderDictionary(),
            ContentType = mimeType
        };

        return testFile;
    }
}