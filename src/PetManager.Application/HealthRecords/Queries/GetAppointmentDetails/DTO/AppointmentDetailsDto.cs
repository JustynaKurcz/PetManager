namespace PetManager.Application.HealthRecords.Queries.GetAppointmentDetails.DTO;

public record AppointmentDetailsDto(
    Guid AppointmentId,
    string Title,
    string Diagnosis,
    DateTimeOffset AppointmentDate,
    string Notes,
    Guid HealthRecordId
);