using PetManager.Application.Common.Pagination;

namespace PetManager.Api.Endpoints.HealthRecords.Queries.BrowseAppointments;

internal sealed class BrowseAppointmentsEndpointRequest : PaginationRequest
{
    [FromRoute(Name = "healthRecordId")] public Guid HealthRecordId { get; init; }
    [FromQuery] public string Search { get; init; }
}