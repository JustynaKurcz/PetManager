using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails;
using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails.DTO;
using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;

namespace PetManager.Infrastructure.EF.HealthRecords.Queries.GetVaccinationDetails;

internal sealed class GetVaccinationDetailsQueryHandler(IHealthRecordRepository healthRecordRepository)
    : IRequestHandler<GetVaccinationDetailsQuery, VaccinationDetailsDto>
{
    public async Task<VaccinationDetailsDto> Handle(GetVaccinationDetailsQuery query,
        CancellationToken cancellationToken)
    {
        var healthRecord =
            await healthRecordRepository.GetByIdAsync(x => x.Id == query.HealthRecordId, cancellationToken,
                asNoTracking: true)
            ?? throw new HealthRecordNotFoundException(query.HealthRecordId);

        var vaccination = healthRecord.Vaccinations.FirstOrDefault(v => v.Id == query.VaccinationId)
                          ?? throw new VaccinationNotFoundException(query.VaccinationId);

        return vaccination.AsVaccinationDetailsDto();
    }
}