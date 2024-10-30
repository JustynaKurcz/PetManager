using PetManager.Application.Context;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Repositories;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Application.Pets.Commands.CreatePet;

internal sealed class CreatePetCommandHandler(
    IContext context,
    IUserRepository userRepository,
    IPetRepository petRepository,
    IHealthRecordRepository healthRecordRepository)
    : IRequestHandler<CreatePetCommand, CreatePetResponse>
{
    public async Task<CreatePetResponse> Handle(CreatePetCommand command, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(context.UserId, cancellationToken);
        if (user is null)
            throw new UserNotFoundException(context.UserId);

        var pet = Pet.Create(command.Name, command.Species, command.Breed, command.Gender, command.BirthDate,
            context.UserId);
        await petRepository.AddAsync(pet, cancellationToken);

        var healthRecord = HealthRecord.Create(pet.PetId);
        pet.AddHealthRecord(healthRecord.HealthRecordId);
        await healthRecordRepository.AddAsync(healthRecord, cancellationToken);

        return new CreatePetResponse(pet.PetId);
    }
}