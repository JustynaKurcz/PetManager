namespace PetManager.Application.Pets.Queries.GetPetDetails.DTO;

public record PetDetailsDto(
    Guid PetId,
    string Name,
    string Species,
    string Breed,
    string Gender,
    DateTimeOffset DateOfBirth,
    Guid UserId,
    Guid? HealthRecordId,
    string photoUrl
);