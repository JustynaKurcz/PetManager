namespace PetManager.Core.Users.Repositories;

public interface IRoleRepository
{
    Task<Guid> GetRoleIdByNameAsync(string name, CancellationToken cancellationToken);
}