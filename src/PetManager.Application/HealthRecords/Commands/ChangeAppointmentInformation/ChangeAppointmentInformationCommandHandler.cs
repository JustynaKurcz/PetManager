using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;

namespace PetManager.Application.HealthRecords.Commands.ChangeAppointmentInformation;

internal sealed class ChangeAppointmentInformationCommandHandler(
    IHealthRecordRepository healthRecordRepository) : IRequestHandler<ChangeAppointmentInformationCommand>
{
    public async Task Handle(ChangeAppointmentInformationCommand command, CancellationToken cancellationToken)
    {
        var healthRecord =
            await healthRecordRepository.GetAsync(x => x.Id == command.HealthRecordId,
                cancellationToken)
            ?? throw new HealthRecordNotFoundException(command.HealthRecordId);
        
        var appointment = healthRecord.Appointments.SingleOrDefault(a => a.Id == command.AppointmentId)
                          ?? throw new AppointmentNotFoundException(command.AppointmentId);

        appointment.ChangeInformation(command.Diagnosis, command.Notes);
        
        await healthRecordRepository.SaveChangesAsync(cancellationToken);
    }
}