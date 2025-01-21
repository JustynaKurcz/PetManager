using PetManager.Application.HealthRecords.Commands.ChangeAppointmentInformation;

namespace PetManager.Api.Endpoints.HealthRecords.Commands.ChangeAppointmentInformation;

internal sealed class ChangeAppointmentInformationEndpointRequest
{
    [FromRoute(Name = "healthRecordId")] public Guid HealthRecordId { get; init; }
    [FromRoute(Name = "appointmentId")] public Guid AppointmentId { get; init; }
    [FromBody] public ChangeAppointmentInformationCommand Command { get; init; } = default!;
    
}