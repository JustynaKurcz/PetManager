using System.Linq.Expressions;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Repositories;
using PetManager.Infrastructure.EF.DbContext;

namespace PetManager.Infrastructure.EF.Pets.Repositories;

internal class ImageRepository(PetManagerDbContext dbContext) : IImageRepository
{
    private readonly DbSet<Image> _images = dbContext.Set<Image>();

    public async Task AddAsync(Image image, CancellationToken cancellationToken)
        => await _images.AddAsync(image, cancellationToken);

    public async Task<Image> GetByIdAsync(Expression<Func<Image, bool>> predicate, CancellationToken cancellationToken)
        => await _images.SingleOrDefaultAsync(predicate, cancellationToken);
    public async Task DeleteAsync(Image image, CancellationToken cancellationToken)
        => _images.Remove(image);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await dbContext.SaveChangesAsync(cancellationToken);
}