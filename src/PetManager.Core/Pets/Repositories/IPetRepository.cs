using PetManager.Core.Pets.Entities;

namespace PetManager.Core.Pets.Repositories;

public interface IPetRepository
{
    Task AddAsync(Pet pet, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<Pet?> GetByIdAsync(Guid petId, CancellationToken cancellationToken, bool asNoTracking = false);
    Task DeleteAsync(Pet pet, CancellationToken cancellationToken);
}