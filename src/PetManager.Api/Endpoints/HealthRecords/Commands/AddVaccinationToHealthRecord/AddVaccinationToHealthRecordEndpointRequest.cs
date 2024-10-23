using PetManager.Application.HealthRecords.Commands.AddVaccinationToHealthRecord;

namespace PetManager.Api.Endpoints.HealthRecords.Commands.AddVaccinationToHealthRecord;

internal sealed class AddVaccinationToHealthRecordEndpointRequest
{
    [FromRoute(Name = "healthRecordId")] public Guid HealthRecordId { get; init; }
    [FromBody] public AddVaccinationToHealthRecordCommand Command { get; init; } = default!;
}