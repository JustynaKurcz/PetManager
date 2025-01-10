using PetManager.Core.Pets.Entities;

namespace PetManager.Core.Pets.Repositories;

public interface IImageRepository
{
    Task AddAsync(Image image, CancellationToken cancellationToken);
    Task<Image?> GetByIdAsync(Expression<Func<Image, bool>> predicate, CancellationToken cancellationToken);
    Task DeleteAsync(Image image, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}