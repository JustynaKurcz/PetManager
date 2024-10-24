using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;

namespace PetManager.Application.HealthRecords.Commands.DeleteVaccinationToHealthRecord;

internal sealed class DeleteVaccinationToHealthRecordCommandHandler(IHealthRecordRepository healthRecordRepository)
    : IRequestHandler<DeleteVaccinationToHealthRecordCommand>
{
    public async Task Handle(DeleteVaccinationToHealthRecordCommand command, CancellationToken cancellationToken)
    {
        var healthRecord =
            await healthRecordRepository.GetHealthRecordByIdAsync(command.HealthRecordId, cancellationToken)
            ?? throw new HealthRecordNotFoundException(command.HealthRecordId);

        var vaccination = healthRecord.Vaccinations.SingleOrDefault(v => v.VaccinationId == command.VaccinationId)
                          ?? throw new VaccinationNotFoundException(command.VaccinationId);

        healthRecord.DeleteVaccination(vaccination);

        await healthRecordRepository.UpdateAsync(healthRecord, cancellationToken);
        await healthRecordRepository.SaveChangesAsync(cancellationToken);
    }
}