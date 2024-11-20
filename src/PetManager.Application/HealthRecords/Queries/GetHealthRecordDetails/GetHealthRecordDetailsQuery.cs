using PetManager.Application.HealthRecords.Queries.GetHealthRecordDetails.DTO;

namespace PetManager.Application.HealthRecords.Queries.GetHealthRecordDetails;

public record GetHealthRecordDetailsQuery(Guid HealthRecordId) : IRequest<HealthRecordDetailsDto>;