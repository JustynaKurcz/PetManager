using PetManager.Application.Common.Context;
using PetManager.Application.Common.Pagination;
using PetManager.Application.HealthRecords.Queries.BrowseVaccinations;
using PetManager.Application.HealthRecords.Queries.BrowseVaccinations.DTO;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;

namespace PetManager.Infrastructure.EF.HealthRecords.Queries.BrowseVaccinations;

internal sealed class BrowseVaccinationsQueryHandler(
    IContext context,
    IVaccinationRepository vaccinationRepository,
    IHealthRecordRepository healthRecordRepository
) : IRequestHandler<BrowseVaccinationsQuery, PaginationResult<VaccinationDto>>
{
    public async Task<PaginationResult<VaccinationDto>> Handle(BrowseVaccinationsQuery query,
        CancellationToken cancellationToken)
    {
        var healthRecord = await healthRecordRepository.GetAsync(x => x.Id == query.HealthRecordId, cancellationToken);
        if (healthRecord is null) throw new HealthRecordNotFoundException(query.HealthRecordId);

        var currentLoggedUserId = context.UserId;
        var vaccinations = await vaccinationRepository.BrowseAsync(currentLoggedUserId, cancellationToken);

        vaccinations = Search(query, vaccinations);

        return await vaccinations
            .Select(x => x.AsVaccinationDto())
            .PaginateAsync(query, cancellationToken);
    }

    private IEnumerable<Vaccination> Search(BrowseVaccinationsQuery query, IEnumerable<Vaccination> vaccinations)
    {
        if (string.IsNullOrWhiteSpace(query.Search)) return vaccinations;
        var searchTxt = $"%{query.Search}%";
        return vaccinations.Where(vaccination =>
            Microsoft.EntityFrameworkCore.EF.Functions.ILike(vaccination.VaccinationName, searchTxt));
    }
}