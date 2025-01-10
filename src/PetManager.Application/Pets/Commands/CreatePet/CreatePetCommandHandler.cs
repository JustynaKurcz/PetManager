using PetManager.Application.Common.Context;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Repositories;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Application.Pets.Commands.CreatePet;

internal sealed class CreatePetCommandHandler(
    IContext context,
    IUserRepository userRepository,
    IPetRepository petRepository
) : IRequestHandler<CreatePetCommand, CreatePetResponse>
{
    public async Task<CreatePetResponse> Handle(CreatePetCommand command, CancellationToken cancellationToken = default)
    {
        var userExists = await userRepository.ExistsAsync(u => u.Id == context.UserId, cancellationToken);
        if (!userExists)
            throw new UserNotFoundException(context.UserId);

        var pet = Pet.Create(command.Name, command.Species, command.Breed, command.Gender, command.BirthDate,
            context.UserId, HealthRecord.Create());
        await petRepository.AddAsync(pet, cancellationToken);

        return new CreatePetResponse(pet.Id);
    }
}