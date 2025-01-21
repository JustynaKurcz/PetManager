namespace PetManager.Application.HealthRecords.Commands.AddVaccinationToHealthRecord;

internal record AddVaccinationToHealthRecordCommand(
    string VaccinationName,
    DateTimeOffset VaccinationDate,
    DateTimeOffset? NextVaccinationDate
) : IRequest<AddVaccinationToHealthRecordResponse>
{
    internal Guid HealthRecordId { get; init; }
}