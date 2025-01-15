namespace PetManager.Application.Pets.Queries.BrowsePets.DTO;

public record PetDto(
    Guid PetId,
    string Name,
    string PhotoUrl
);