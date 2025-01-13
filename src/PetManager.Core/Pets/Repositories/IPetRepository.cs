using PetManager.Core.Pets.Entities;

namespace PetManager.Core.Pets.Repositories;

public interface IPetRepository
{
    Task AddAsync(Pet pet, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task<Pet?> GetAsync(Expression<Func<Pet, bool>> predicate, CancellationToken cancellationToken,
        bool asNoTracking = false);

    Task<IQueryable<Pet>> BrowseAsync(Guid userId, CancellationToken cancellationToken);
    Task DeleteAsync(Pet pet, CancellationToken cancellationToken);
}