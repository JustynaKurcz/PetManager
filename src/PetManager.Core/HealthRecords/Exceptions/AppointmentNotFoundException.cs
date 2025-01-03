using PetManager.Core.Common.Exceptions;

namespace PetManager.Core.HealthRecords.Exceptions;

public sealed class AppointmentNotFoundException : PetManagerException
{
    public Guid AppointmentId { get; }

    public AppointmentNotFoundException(Guid appointmentId) : base(
        $"Appointment with ID {appointmentId} was not found.")
    {
        AppointmentId = appointmentId;
    }
}