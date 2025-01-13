namespace PetManager.Application.HealthRecords.Queries.BrowseAppointments.DTO;

public record AppointmentDto(
    Guid Id,
    string Title,
    DateTimeOffset AppointmentDate,
    bool IsNotificationSent
);