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

    public async Task<Pet> GetByIdAsync(Guid petId, CancellationToken cancellationToken, bool asNoTracking = false)
    {
        var query = _pets.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.SingleOrDefaultAsync(x => x.PetId == petId, cancellationToken);
    }

    public async Task<IQueryable<Pet>> BrowseAsync(CancellationToken cancellationToken)
        =>  _pets.AsQueryable();

    public async Task DeleteAsync(Pet pet, CancellationToken cancellationToken)
    {
        _pets.Remove(pet);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}