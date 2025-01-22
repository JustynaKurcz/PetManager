using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Core.HealthRecords.Repositories;

public interface IVaccinationRepository
{
    Task<IEnumerable<Vaccination>> GetScheduledVaccinationsAsync(int reminderDays, CancellationToken cancellationToken);
    Task<IEnumerable<Vaccination>> BrowseAsync(Guid healthRecordId, Guid userId, CancellationToken cancellationToken);
    Task UpdateVaccinationAsync(Vaccination vaccination, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}