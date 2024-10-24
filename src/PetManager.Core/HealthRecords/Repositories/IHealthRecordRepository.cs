using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Core.HealthRecords.Repositories;

public interface IHealthRecordRepository
{
    Task AddAsync(HealthRecord healthRecord, CancellationToken cancellationToken);

    Task<HealthRecord?> GetByIdAsync(Guid healthRecordId, CancellationToken cancellationToken,
        bool asNoTracking = false);

    Task UpdateAsync(HealthRecord healthRecord, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}