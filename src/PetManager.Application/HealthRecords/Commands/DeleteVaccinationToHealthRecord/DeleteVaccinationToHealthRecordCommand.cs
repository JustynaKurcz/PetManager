namespace PetManager.Application.HealthRecords.Commands.DeleteVaccinationToHealthRecord;

internal record DeleteVaccinationToHealthRecordCommand(Guid HealthRecordId, Guid VaccinationId) : IRequest;