using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Infrastructure.EF.DbContext;

namespace PetManager.Infrastructure.EF.HealthRecords.Repositories;

internal class AppointmentRepository(PetManagerDbContext dbContext) : IAppointmentRepository
{
    private readonly DbSet<Appointment> _appointments = dbContext.Appointments;

    public async Task<IEnumerable<Appointment>> GetScheduledAppointmentsAsync(int reminderDays,
        CancellationToken cancellationToken)
    {
        var today = DateTimeOffset.UtcNow;
        var nextWeek = today.AddDays(reminderDays);

        return await _appointments
            .Include(a => a.HealthRecord)
            .ThenInclude(hr => hr.Pet)
            .ThenInclude(p => p.User)
            .Where(a => a.AppointmentDate >= today && a.AppointmentDate <= nextWeek && !a.IsNotificationSent)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> BrowseAsync(Guid userId, CancellationToken cancellationToken)
        => await Task.FromResult(
            _appointments
                .Include(a => a.HealthRecord)
                .ThenInclude(hr => hr.Pet)
                .ThenInclude(p => p.User)
                .Where(x=>x.HealthRecord.Pet.UserId == userId)
                .AsSplitQuery()
            );

    public async Task UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
        => await Task.FromResult(_appointments.Update(appointment));

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await dbContext.SaveChangesAsync(cancellationToken);
}