using PetManager.Application.Common.Pagination;
using PetManager.Application.HealthRecords.Queries.BrowseAppointments.DTO;

namespace PetManager.Application.HealthRecords.Queries.BrowseAppointments;

internal sealed class BrowseAppointmentsQuery : PaginationRequest, IRequest<PaginationResult<AppointmentDto>>
{
    public string Search { get; set; }
    public Guid HealthRecordId { get; init; }
}