namespace PetManager.Api.Endpoints.HealthRecords.Queries.GetVaccinationDetails;

internal sealed class GetVaccinationDetailsEndpointRequest
{
    [FromRoute(Name = "healthRecordId")] public Guid HealthRecordId { get; init; }
    [FromRoute(Name = "vaccinationId")] public Guid VaccinationId { get; init; }
}