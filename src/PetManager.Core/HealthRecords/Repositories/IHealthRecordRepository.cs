using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Core.HealthRecords.Repositories;

public interface IHealthRecordRepository
{
    Task<HealthRecord?> GetAsync(Expression<Func<HealthRecord, bool>> predicate,
        CancellationToken cancellationToken,
        bool asNoTracking = false);

    Task UpdateAsync(HealthRecord healthRecord, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}