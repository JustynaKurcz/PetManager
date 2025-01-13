using System.Linq.Expressions;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Repositories;
using PetManager.Infrastructure.EF.DbContext;

namespace PetManager.Infrastructure.EF.Pets.Repositories;

internal class PetRepository(PetManagerDbContext dbContext) : IPetRepository
{
    private readonly DbSet<Pet> _pets = dbContext.Set<Pet>();

    public async Task AddAsync(Pet pet, CancellationToken cancellationToken)
    {
        await _pets.AddAsync(pet, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await dbContext.SaveChangesAsync(cancellationToken);

    public async Task<Pet?> GetByIdAsync(Expression<Func<Pet, bool>> predicate, CancellationToken cancellationToken,
        bool asNoTracking = false)
    {
        var query = _pets
            .Include(x => x.Image)
            .AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query
            .AsSplitQuery()
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IQueryable<Pet>> BrowseAsync(CancellationToken cancellationToken)
        => await Task.FromResult(_pets
            .Include(q => q.User)
            .AsSplitQuery()
            .AsQueryable()
        );

    public async Task DeleteAsync(Pet pet, CancellationToken cancellationToken)
    {
        _pets.Remove(pet);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}