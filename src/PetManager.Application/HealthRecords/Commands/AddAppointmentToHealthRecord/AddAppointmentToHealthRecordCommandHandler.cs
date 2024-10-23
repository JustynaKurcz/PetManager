using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;

namespace PetManager.Application.HealthRecords.Commands.AddAppointmentToHealthRecord;

internal sealed class AddAppointmentToHealthRecordCommandHandler(IHealthRecordRepository healthRecordRepository)
    : IRequestHandler<AddAppointmentToHealthRecordCommand, AddAppointmentToHealthRecordResponse>
{
    public async Task<AddAppointmentToHealthRecordResponse> Handle(AddAppointmentToHealthRecordCommand command,
        CancellationToken cancellationToken)
    {
        var healthRecord =
            await healthRecordRepository.GetHealthRecordByIdAsync(command.HealthRecordId, cancellationToken)
            ?? throw new HealthRecordNotFoundException(command.HealthRecordId);

        var appointment = Appointment.Create(command.Title, command.Diagnosis, command.AppointmentDate, command.Notes,
            healthRecord.HealthRecordId);

        healthRecord.AddAppointment(appointment);

        await healthRecordRepository.UpdateAsync(healthRecord, cancellationToken);
        await healthRecordRepository.SaveChangesAsync(cancellationToken);

        return new AddAppointmentToHealthRecordResponse(appointment.AppointmentId);
    }
}