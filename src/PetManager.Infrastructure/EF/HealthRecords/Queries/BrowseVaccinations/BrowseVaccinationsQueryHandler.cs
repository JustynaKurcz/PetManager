using PetManager.Application.Common.Pagination;
using PetManager.Application.HealthRecords.Queries.BrowseVaccinations;
using PetManager.Application.HealthRecords.Queries.BrowseVaccinations.DTO;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Repositories;

namespace PetManager.Infrastructure.EF.HealthRecords.Queries.BrowseVaccinations;

internal sealed class BrowseVaccinationsQueryHandler(IVaccinationRepository vaccinationRepository)
    : IRequestHandler<BrowseVaccinationsQuery, PaginationResult<VaccinationDto>>
{
    public async Task<PaginationResult<VaccinationDto>> Handle(BrowseVaccinationsQuery query, CancellationToken cancellationToken)
    {
        var vaccinations = await vaccinationRepository.BrowseAsync(cancellationToken);

        vaccinations = Search(query, vaccinations);

        return await vaccinations.AsNoTracking()
            .Select(x => x.AsVaccinationDto())
            .PaginateAsync(query, cancellationToken);
    }

    private IQueryable<Vaccination> Search(BrowseVaccinationsQuery query, IQueryable<Vaccination> vaccinations)
    {
        if (string.IsNullOrWhiteSpace(query.Search)) return vaccinations;
        var searchTxt = $"%{query.Search}%";
        return vaccinations.Where(vaccination =>
            Microsoft.EntityFrameworkCore.EF.Functions.ILike(vaccination.VaccinationName, searchTxt));
    }
}