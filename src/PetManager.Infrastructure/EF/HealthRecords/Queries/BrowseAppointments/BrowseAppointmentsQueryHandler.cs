using PetManager.Application.Common.Pagination;
using PetManager.Application.HealthRecords.Queries.BrowseAppointments;
using PetManager.Application.HealthRecords.Queries.BrowseAppointments.DTO;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Repositories;

namespace PetManager.Infrastructure.EF.HealthRecords.Queries.BrowseAppointments;

internal sealed class BrowseAppointmentsQueryHandler(IAppointmentRepository appointmentRepository)
    : IRequestHandler<BrowseAppointmentsQuery, PaginationResult<AppointmentDto>>
{
    public async Task<PaginationResult<AppointmentDto>> Handle(BrowseAppointmentsQuery query, CancellationToken cancellationToken)
    {
        var appointments = await appointmentRepository.BrowseAsync(cancellationToken);

        appointments = Search(query, appointments);

        return await appointments.AsNoTracking()
            .Select(x => x.AsAppointmentDto())
            .PaginateAsync(query, cancellationToken);
    }
    
    private IQueryable<Appointment> Search(BrowseAppointmentsQuery query, IQueryable<Appointment> appointments)
    {
        if (string.IsNullOrWhiteSpace(query.Search)) return appointments;
        var searchTxt = $"%{query.Search}%";
        return appointments.Where(appointment =>
            Microsoft.EntityFrameworkCore.EF.Functions.ILike(appointment.Title, searchTxt) ||
            Microsoft.EntityFrameworkCore.EF.Functions.ILike(appointment.Diagnosis, searchTxt));
    }
}