using PetManager.Application.Common.Pagination;

namespace PetManager.Api.Endpoints.HealthRecords.Queries.BrowseVaccinations;

internal sealed class BrowseVaccinationsEndpointRequest : PaginationRequest
{
    [FromRoute(Name = "healthRecordId")] public Guid HealthRecordId { get; init; }
    [FromQuery] public string Search { get; init; }
}