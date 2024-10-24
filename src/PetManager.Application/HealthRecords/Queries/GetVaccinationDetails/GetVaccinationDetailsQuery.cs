using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails.DTO;

namespace PetManager.Application.HealthRecords.Queries.GetVaccinationDetails;

public record GetVaccinationDetailsQuery(Guid HealthRecordId, Guid VaccinationId) : IRequest<VaccinationDetailsDto>;