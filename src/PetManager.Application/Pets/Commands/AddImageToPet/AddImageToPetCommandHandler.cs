using PetManager.Application.Common.Integrations.BlobStorage;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Exceptions;
using PetManager.Core.Pets.Repositories;

namespace PetManager.Application.Pets.Commands.AddImageToPet;

internal sealed class AddImageToPetCommandHandler(
    IBlobStorageService blobStorageService,
    IPetRepository petRepository,
    IImageRepository imageRepository
) : IRequestHandler<AddImageToPetCommand, AddImageToPetResponse>
{
    public async Task<AddImageToPetResponse> Handle(AddImageToPetCommand command, CancellationToken cancellationToken)
    {
        var pet = await petRepository.GetAsync(x => x.Id == command.PetId, cancellationToken)
                  ?? throw new PetNotFoundException(command.PetId);

        if (pet.Image is not null)
        {
            await blobStorageService.DeleteImageAsync(pet.Image!.BlobUrl, cancellationToken);

            await imageRepository.DeleteAsync(pet.Image, cancellationToken);
            await imageRepository.SaveChangesAsync(cancellationToken);
        }

        var blobUrl = await blobStorageService.UploadImageAsync(command.File, cancellationToken);

        var image = Image.Create(
            command.File.FileName,
            blobUrl,
            pet.Id
        );

        pet.SetImage(image);

        await imageRepository.AddAsync(image, cancellationToken);
        await imageRepository.SaveChangesAsync(cancellationToken);

        return new AddImageToPetResponse(image.Id);
    }
}