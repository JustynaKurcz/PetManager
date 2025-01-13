namespace PetManager.Application.HealthRecords.Queries.BrowseVaccinations.DTO;

public record VaccinationDto(
    Guid Id,
    string VaccinationName,
    DateTimeOffset VaccinationDate,
    DateTimeOffset NextVaccinationDate,
    bool IsNotificationSent
);