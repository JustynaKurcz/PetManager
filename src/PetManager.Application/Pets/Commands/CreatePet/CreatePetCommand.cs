using PetManager.Core.Pets.Enums;

namespace PetManager.Application.Pets.Commands.CreatePet;

public record CreatePetCommand(
    string Name,
    Species Species,
    string Breed,
    Gender Gender,
    DateTimeOffset BirthDate,
    Guid UserId
) : IRequest<CreatePetResponse>;