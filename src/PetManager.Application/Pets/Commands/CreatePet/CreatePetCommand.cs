using PetManager.Core.Pets.Enums;

namespace PetManager.Application.Pets.Commands.CreatePet;

internal record CreatePetCommand(
    string Name,
    Species Species,
    string Breed,
    Gender Gender,
    DateTimeOffset BirthDate
) : IRequest<CreatePetResponse>;