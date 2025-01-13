using PetManager.Application.HealthRecords.Queries.GetHealthRecordDetails;
using PetManager.Application.HealthRecords.Queries.GetHealthRecordDetails.DTO;
using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;

namespace PetManager.Infrastructure.EF.HealthRecords.Queries.GetHealthRecordDetails;

internal sealed class GetHealthRecordDetailsQueryHandler(IHealthRecordRepository healthRecordRepository)
    : IRequestHandler<GetHealthRecordDetailsQuery, HealthRecordDetailsDto>
{
    public async Task<HealthRecordDetailsDto> Handle(GetHealthRecordDetailsQuery query,
        CancellationToken cancellationToken)
    {
        var healthRecord = await healthRecordRepository.GetAsync(x => x.Id == query.HealthRecordId,
                               cancellationToken, asNoTracking: true)
                           ?? throw new HealthRecordNotFoundException(query.HealthRecordId);

        return healthRecord.AsDetailsDto();
    }
}