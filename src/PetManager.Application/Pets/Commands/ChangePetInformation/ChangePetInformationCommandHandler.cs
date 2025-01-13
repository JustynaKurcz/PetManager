using PetManager.Application.Common.Context;
using PetManager.Core.Pets.Exceptions;
using PetManager.Core.Pets.Repositories;

namespace PetManager.Application.Pets.Commands.ChangePetInformation;

internal sealed class ChangePetInformationCommandHandler(
    IContext context,
    IPetRepository petRepository
) : IRequestHandler<ChangePetInformationCommand>
{
    public async Task Handle(ChangePetInformationCommand command, CancellationToken cancellationToken)
    {
        var currentLoggedUserId = context.UserId;

        var pet = await petRepository.GetAsync(x => x.Id == command.PetId, cancellationToken)
                  ?? throw new PetNotFoundException(command.PetId);

        var isPetOwnedByCurrentUser = pet.UserId == currentLoggedUserId;
        if (!isPetOwnedByCurrentUser)
            throw new PetNotOwnedException(command.PetId);

        pet.ChangeInformation(command.Species, command.Breed, command.Gender);

        await petRepository.SaveChangesAsync(cancellationToken);
    }
}