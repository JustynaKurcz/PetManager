namespace PetManager.Application.HealthRecords.Commands.DeleteAppointmentToHealthRecord;

internal record DeleteAppointmentToHealthRecordCommand(Guid HealthRecordId, Guid AppointmentId) : IRequest;