using PetManager.Core.Pets.Enums;

namespace PetManager.Application.Pets.Queries.GetPetDetails.DTO;

public record PetDetailsDto(
    Guid PetId,
    string Name,
    Species Species,
    string Breed,
    Gender Gender,
    DateTimeOffset DateOfBirth,
    Guid UserId,
    Guid? HealthRecordId
);