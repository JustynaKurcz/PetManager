using PetManager.Core.Users.Entities;

namespace PetManager.Core.Users.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken);
    Task DeleteAsync(User user, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}