using PetManager.Application.Common.Context;
using PetManager.Application.Common.Pagination;
using PetManager.Application.HealthRecords.Queries.BrowseAppointments;
using PetManager.Application.HealthRecords.Queries.BrowseAppointments.DTO;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;

namespace PetManager.Infrastructure.EF.HealthRecords.Queries.BrowseAppointments;

internal sealed class BrowseAppointmentsQueryHandler(
    IContext context,
    IAppointmentRepository appointmentRepository,
    IHealthRecordRepository healthRecordRepository
) : IRequestHandler<BrowseAppointmentsQuery, PaginationResult<AppointmentDto>>
{
    public async Task<PaginationResult<AppointmentDto>> Handle(BrowseAppointmentsQuery query,
        CancellationToken cancellationToken)
    {
        var healthRecord = await healthRecordRepository.GetAsync(x => x.Id == query.HealthRecordId, cancellationToken);
        if (healthRecord is null) throw new HealthRecordNotFoundException(query.HealthRecordId);

        var currentLoggedUserId = context.UserId;
        var appointments = await appointmentRepository.BrowseAsync(query.HealthRecordId, currentLoggedUserId, cancellationToken);

        appointments = Search(query, appointments);

        return await appointments
            .Select(x => x.AsAppointmentDto())
            .PaginateAsync(query, cancellationToken);
    }

    private IEnumerable<Appointment> Search(BrowseAppointmentsQuery query, IEnumerable<Appointment> appointments)
    {
        if (string.IsNullOrWhiteSpace(query.Search)) return appointments;
        var searchTxt = $"%{query.Search}%";
        return appointments.Where(appointment =>
            Microsoft.EntityFrameworkCore.EF.Functions.ILike(appointment.Title, searchTxt) ||
            Microsoft.EntityFrameworkCore.EF.Functions.ILike(appointment.Diagnosis, searchTxt));
    }
}