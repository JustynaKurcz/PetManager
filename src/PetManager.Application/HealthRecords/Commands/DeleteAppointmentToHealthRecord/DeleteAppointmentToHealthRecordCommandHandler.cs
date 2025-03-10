using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;

namespace PetManager.Application.HealthRecords.Commands.DeleteAppointmentToHealthRecord;

internal sealed class DeleteAppointmentToHealthRecordCommandHandler(IHealthRecordRepository healthRecordRepository)
    : IRequestHandler<DeleteAppointmentToHealthRecordCommand>
{
    public async Task Handle(DeleteAppointmentToHealthRecordCommand command, CancellationToken cancellationToken)
    {
        var healthRecord =
            await healthRecordRepository.GetAsync(x => x.Id == command.HealthRecordId,
                cancellationToken)
            ?? throw new HealthRecordNotFoundException(command.HealthRecordId);

        var appointment = healthRecord.Appointments.SingleOrDefault(a => a.Id == command.AppointmentId)
                          ?? throw new AppointmentNotFoundException(command.AppointmentId);

        healthRecord.DeleteAppointment(appointment);

        await healthRecordRepository.UpdateAsync(healthRecord, cancellationToken);
        await healthRecordRepository.SaveChangesAsync(cancellationToken);
    }
}