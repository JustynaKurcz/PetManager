using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;

namespace PetManager.Application.HealthRecords.Commands.ChangeVaccinationInformation;

internal sealed class ChangeVaccinationInformationCommandHandler(
    IHealthRecordRepository healthRecordRepository) : IRequestHandler<ChangeVaccinationInformationCommand>
{
    public async Task Handle(ChangeVaccinationInformationCommand command, CancellationToken cancellationToken)
    {
        var healthRecord =
            await healthRecordRepository.GetAsync(x => x.Id == command.HealthRecordId,
                cancellationToken)
            ?? throw new HealthRecordNotFoundException(command.HealthRecordId);

        var vaccination = healthRecord.Vaccinations.SingleOrDefault(a => a.Id == command.VaccinationId)
                          ?? throw new VaccinationNotFoundException(command.VaccinationId);
        
        vaccination.ChangeInformation(command.NextVaccinationDate);
        
        await healthRecordRepository.UpdateAsync(healthRecord, cancellationToken);
        await healthRecordRepository.SaveChangesAsync(cancellationToken);
    }
}