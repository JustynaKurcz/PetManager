using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Infrastructure.EF.Context;

namespace PetManager.Infrastructure.EF.HealthRecords.Repositories;

internal class HealthRecordRepository(PetManagerDbContext dbContext) : IHealthRecordRepository
{
    private readonly DbSet<HealthRecord> _healthRecords = dbContext.HealthRecords;

    public async Task AddAsync(HealthRecord healthRecord, CancellationToken cancellationToken)
    {
        await _healthRecords.AddAsync(healthRecord, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<HealthRecord?> GetByIdAsync(Guid healthRecordId, CancellationToken cancellationToken,
        bool asNoTracking = false)
    {
        var query = _healthRecords.AsQueryable()
            .Include(x => x.Appointments)
            .Include(x => x.Vaccinations)
            .AsSplitQuery();

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.SingleOrDefaultAsync(x => x.HealthRecordId == healthRecordId, cancellationToken);
    }

    public async Task UpdateAsync(HealthRecord healthRecord, CancellationToken cancellationToken)
    {
        UpdateEntityState(healthRecord.Vaccinations);
        UpdateEntityState(healthRecord.Appointments);

        await Task.FromResult(_healthRecords.Update(healthRecord));
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await dbContext.SaveChangesAsync(cancellationToken);

    private void UpdateEntityState<T>(IEnumerable<T> entities) where T : class
    {
        foreach (var entity in entities)
        {
            var entry = dbContext.Entry(entity);
            if (entry.State == EntityState.Modified || entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
        }
    }
}