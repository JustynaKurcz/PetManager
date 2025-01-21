namespace PetManager.Application.HealthRecords.Commands.AddAppointmentToHealthRecord;

internal record AddAppointmentToHealthRecordCommand(
    string Title,
    string? Diagnosis,
    DateTimeOffset AppointmentDate,
    string? Notes
) : IRequest<AddAppointmentToHealthRecordResponse>
{
    internal Guid HealthRecordId { get; init; }
}