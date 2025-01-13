using PetManager.Application.Users.Admin.Queries.BrowseUsers.DTO;
using PetManager.Core.Users.Entities;

namespace PetManager.Infrastructure.EF.Users.Queries.Admin.Queries;

internal static class AdminExtensions
{
    public static UserDto AsUserDto (this User user)
        => new(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.CreatedAt,
            user.Pets.Count()
        );
    
}