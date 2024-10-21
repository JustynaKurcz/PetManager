using PetManager.Core.Users.Entities;

namespace PetManager.Core.Users.Repositories;

public interface IRoleRepository
{
    Task<Role> GetRoleByNameAsync(string name, CancellationToken cancellationToken);
}