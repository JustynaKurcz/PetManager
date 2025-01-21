namespace PetManager.Application.HealthRecords.Commands.ChangeAppointmentInformation;

internal record class ChangeAppointmentInformationCommand(
    string Diagnosis,
    string Notes
) : IRequest
{
    internal Guid HealthRecordId { get; init; }
    internal Guid AppointmentId { get; init; }
}