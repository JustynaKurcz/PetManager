using PetManager.Core.Pets.Exceptions;
using PetManager.Core.Pets.Repositories;

namespace PetManager.Application.Pets.Commands.DeletePet;

internal sealed class DeletePetCommandHandler(IPetRepository petRepository) : IRequestHandler<DeletePetCommand>
{
    public async Task Handle(DeletePetCommand command, CancellationToken cancellationToken)
    {
        var pet = await petRepository.GetByIdAsync(x => x.Id == command.PetId, cancellationToken)
                  ?? throw new PetNotFoundException(command.PetId);

        await petRepository.DeleteAsync(pet, cancellationToken);
    }
}