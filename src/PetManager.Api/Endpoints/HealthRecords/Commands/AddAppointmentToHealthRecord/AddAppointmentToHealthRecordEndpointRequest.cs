using PetManager.Application.HealthRecords.Commands.AddAppointmentToHealthRecord;

namespace PetManager.Api.Endpoints.HealthRecords.Commands.AddAppointmentToHealthRecord;

internal sealed class AddAppointmentToHealthRecordEndpointRequest
{
    [FromRoute(Name = "healthRecordId")] public Guid HealthRecordId { get; init; }
    [FromBody] public AddAppointmentToHealthRecordCommand Command { get; init; } = default!;
}