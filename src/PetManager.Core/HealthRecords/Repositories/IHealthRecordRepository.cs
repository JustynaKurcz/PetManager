using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Core.HealthRecords.Repositories;

public interface IHealthRecordRepository
{
    Task AddAsync(HealthRecord healthRecord, CancellationToken cancellationToken);
    Task<HealthRecord?> GetHealthRecordByIdAsync(Guid healthRecordId, CancellationToken cancellationToken);
    Task UpdateAsync(HealthRecord healthRecord, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}