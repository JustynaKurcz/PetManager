using PetManager.Application.Common.Pagination;
using PetManager.Application.HealthRecords.Queries.BrowseVaccinations.DTO;

namespace PetManager.Application.HealthRecords.Queries.BrowseVaccinations;

internal sealed class BrowseVaccinationsQuery : PaginationRequest, IRequest<PaginationResult<VaccinationDto>>
{
    public string Search { get; set; }
}