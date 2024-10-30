using PetManager.Core.Users.Enums;

namespace PetManager.Application.Users.Queries.GetCurrentUserDetails.DTO;

public record CurrentUserDetailsDto(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    DateTimeOffset? LastChangePasswordDate,
    DateTimeOffset CreatedAt,
    UserRole Role
);