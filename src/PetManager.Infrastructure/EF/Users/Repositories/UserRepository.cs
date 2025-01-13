using System.Linq.Expressions;
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

    public async Task<User?> GetByEmailAsync(Expression<Func<User, bool>> predicate,
        CancellationToken cancellationToken)
        => await _users
            .FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task<User?> GetByIdAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken)
        => await _users.SingleOrDefaultAsync(predicate, cancellationToken);

    public async Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken)
        => await _users
            .AnyAsync(predicate, cancellationToken);

    public async Task<IQueryable<User>> BrowseAsync(CancellationToken cancellationToken)
        => await Task.FromResult(
            _users
                .Include(x => x.Pets)
                .AsSplitQuery()
                .AsQueryable()
        );

    public async Task DeleteAsync(User user, CancellationToken cancellationToken)
    {
        _users.Remove(user);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await dbContext.SaveChangesAsync(cancellationToken);
}