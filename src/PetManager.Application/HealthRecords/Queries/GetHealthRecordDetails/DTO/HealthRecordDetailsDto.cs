using PetManager.Application.HealthRecords.Queries.GetAppointmentDetails.DTO;
using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails.DTO;

namespace PetManager.Application.HealthRecords.Queries.GetHealthRecordDetails.DTO;

public record HealthRecordDetailsDto(
    Guid HealthRecordId,
    Guid PetId,
    string Notes,
    IReadOnlyCollection<VaccinationDetailsDto> Vaccinations,
    IReadOnlyCollection<AppointmentDetailsDto> Appointments
);