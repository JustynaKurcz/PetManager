using PetManager.Core.HealthRecords.Entitites;

namespace PetManager.Core.HealthRecords.Repositories;

public interface IHealthRepository
{
    Task AddAsync(HealthRecord healthRecord, CancellationToken cancellationToken);
}