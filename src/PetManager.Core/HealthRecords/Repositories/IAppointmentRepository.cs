using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Core.HealthRecords.Repositories;

public interface IAppointmentRepository
{
    Task<IEnumerable<Appointment>> GetScheduledAppointmentsAsync(int reminderDays, CancellationToken cancellationToken);
    Task<IQueryable<Appointment>> BrowseAsync(Guid userId, CancellationToken cancellationToken);
    Task UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}