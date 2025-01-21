namespace PetManager.Application.HealthRecords.Commands.ChangeVaccinationInformation;

internal record ChangeVaccinationInformationCommand(
    DateTimeOffset NextVaccinationDate
) : IRequest
{
    internal Guid HealthRecordId { get; init; }
    internal Guid VaccinationId { get; init; }
}