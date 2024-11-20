using PetManager.Core.Pets.Enums;

namespace PetManager.Application.Pets.Commands.ChangePetInformation;

internal record ChangePetInformationCommand(
    Species Species,
    string Breed,
    Gender Gender
) : IRequest
{
    internal Guid PetId { get; init; }
}