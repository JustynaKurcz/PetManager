using PetManager.Application.Users.Queries.GetCurrentUserDetails.DTO;
using PetManager.Core.Users.Entities;
using PetManager.Infrastructure.EF.Pets.Queries;

namespace PetManager.Infrastructure.EF.Users.Queries;

internal static class Extensions
{
    public static CurrentUserDetailsDto AsDetailsDto(this User user)
        => new(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.LastChangePasswordDate,
            user.CreatedAt,
            user.Role.GetDisplayName(),
            user.Pets.Count()
        );
}