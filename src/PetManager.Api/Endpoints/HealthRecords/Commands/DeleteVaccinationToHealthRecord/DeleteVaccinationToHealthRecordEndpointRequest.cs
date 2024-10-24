namespace PetManager.Api.Endpoints.HealthRecords.Commands.DeleteVaccinationToHealthRecord;

internal sealed class DeleteVaccinationToHealthRecordEndpointRequest
{
    [FromRoute(Name = "healthRecordId")] public Guid HealthRecordId { get; init; }
    [FromRoute(Name = "vaccinationId")] public Guid VaccinationId { get; init; }
}