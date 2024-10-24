using PetManager.Core.Pets.Enums;

namespace PetManager.Application.Pets.Commands.ChangePetInformation;

internal record ChangePetInformationCommand(
    string Name,
    Species Species,
    string Breed,
    Gender Gender,
    DateTimeOffset BirthDate
) : IRequest
{
    internal Guid PetId { get; init; }
}