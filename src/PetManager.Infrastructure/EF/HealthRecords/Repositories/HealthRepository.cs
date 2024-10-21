using PetManager.Core.HealthRecords.Entitites;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Infrastructure.EF.Context;

namespace PetManager.Infrastructure.EF.HealthRecords.Repositories;

internal class HealthRepository(PetManagerDbContext dbContext) : IHealthRepository
{
    private readonly DbSet<HealthRecord> _healthRecords = dbContext.HealthRecords;

    public async Task AddAsync(HealthRecord healthRecord, CancellationToken cancellationToken)
    {
        await _healthRecords.AddAsync(healthRecord, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}