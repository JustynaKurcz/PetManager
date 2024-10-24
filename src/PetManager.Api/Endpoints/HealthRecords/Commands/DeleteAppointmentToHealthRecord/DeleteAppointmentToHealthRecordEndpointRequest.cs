namespace PetManager.Api.Endpoints.HealthRecords.Commands.DeleteAppointmentToHealthRecord;

internal sealed class DeleteAppointmentToHealthRecordEndpointRequest
{
    [FromRoute(Name = "healthRecordId")] public Guid HealthRecordId { get; init; }
    [FromRoute(Name = "appointmentId")] public Guid AppointmentId { get; init; }
}