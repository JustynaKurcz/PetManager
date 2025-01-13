namespace PetManager.Application.Users.Admin.Queries.BrowseUsers.DTO;

public record UserDto(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    DateTimeOffset CreatedAt,
    int PetsCount
);