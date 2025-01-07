using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Repositories;
using PetManager.Infrastructure.EF.DbContext;

namespace PetManager.Infrastructure.EF.Users.Repositories;

internal class UserRepository(PetManagerDbContext dbContext) : IUserRepository
{
    private readonly DbSet<User> _users = dbContext.Users;

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => await _users
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        => await _users.SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);


    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
        => await _users
            .AnyAsync(x => x.Email == email, cancellationToken);

    public async Task DeleteAsync(User user, CancellationToken cancellationToken)
    {
        _users.Remove(user);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await dbContext.SaveChangesAsync(cancellationToken);
}