namespace PetManager.Api.Endpoints.HealthRecords.Queries.GetAppointmentDetails;

internal sealed class GetAppointmentDetailsEndpointRequest
{
    [FromRoute(Name = "healthRecordId")] public Guid HealthRecordId { get; init; }
    [FromRoute(Name = "appointmentId")] public Guid AppointmentId { get; init; }
}