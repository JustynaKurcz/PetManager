using PetManager.Application.HealthRecords.Commands.ChangeVaccinationInformation;

namespace PetManager.Api.Endpoints.HealthRecords.Commands.ChangeVaccinationInformation;

internal sealed class ChangeVaccinationInformationEndpointRequest
{
    [FromRoute(Name = "healthRecordId")] public Guid HealthRecordId { get; init; }
    [FromRoute(Name = "vaccinationId")] public Guid VaccinationId { get; init; }
    [FromBody] public ChangeVaccinationInformationCommand Command { get; init; } = default!;
}