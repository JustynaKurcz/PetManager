using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;

namespace PetManager.Application.HealthRecords.Commands.AddVaccinationToHealthRecord;

internal sealed class AddVaccinationToHealthRecordCommandHandler(
    IHealthRecordRepository healthRecordRepository)
    : IRequestHandler<AddVaccinationToHealthRecordCommand, AddVaccinationToHealthRecordResponse>
{
    public async Task<AddVaccinationToHealthRecordResponse> Handle(AddVaccinationToHealthRecordCommand command,
        CancellationToken cancellationToken)
    {
        var healthRecord =
            await healthRecordRepository.GetAsync(x => x.Id == command.HealthRecordId,
                cancellationToken)
            ?? throw new HealthRecordNotFoundException(command.HealthRecordId);

        var vaccination = Vaccination.Create(command.VaccinationName, command.VaccinationDate,
           command.NextVaccinationDate, healthRecord.Id);

        healthRecord.AddVaccination(vaccination);

        await healthRecordRepository.UpdateAsync(healthRecord, cancellationToken);
        await healthRecordRepository.SaveChangesAsync(cancellationToken);

        return new AddVaccinationToHealthRecordResponse(vaccination.Id);
    }
}