using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Repositories;
using PetManager.Infrastructure.EF.Context;

namespace PetManager.Infrastructure.EF.Users.Repositories;

internal class RoleRepository(PetManagerDbContext dbContext) : IRoleRepository
{
    private readonly DbSet<Role> _roles = dbContext.Roles;

    public async Task<Role> GetRoleByNameAsync(string name, CancellationToken cancellationToken)
        => (await _roles
            .FirstAsync(x => x.Name == name, cancellationToken));
}