using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Repositories;
using PetManager.Infrastructure.EF.Context;

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

    public async Task<Pet> GetByIdAsync(Guid petId, CancellationToken cancellationToken)
        => await _pets
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.PetId == petId, cancellationToken);
}