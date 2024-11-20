using PetManager.Core.Pets.Exceptions;
using PetManager.Core.Pets.Repositories;

namespace PetManager.Application.Pets.Commands.ChangePetInformation;

internal sealed class ChangePetInformationCommandHandler(IPetRepository petRepository)
    : IRequestHandler<ChangePetInformationCommand>
{
    public async Task Handle(ChangePetInformationCommand command, CancellationToken cancellationToken)
    {
        var pet = await petRepository.GetByIdAsync(command.PetId, cancellationToken)
                  ?? throw new PetNotFoundException(command.PetId);

        pet.ChangeInformation(command.Species, command.Breed, command.Gender);

        await petRepository.SaveChangesAsync(cancellationToken);
    }
}