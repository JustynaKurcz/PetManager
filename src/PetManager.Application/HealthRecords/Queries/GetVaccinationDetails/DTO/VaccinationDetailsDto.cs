namespace PetManager.Application.HealthRecords.Queries.GetVaccinationDetails.DTO;

public record VaccinationDetailsDto(
    Guid VaccinationId,
    string Name,
    DateTimeOffset VaccinationDate,
    DateTimeOffset? NextVaccinationDate,
    Guid HealthRecordId,
    bool IsNotificationSent
);