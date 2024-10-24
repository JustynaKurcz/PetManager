using PetManager.Application.HealthRecords.Queries.GetAppointmentDetails.DTO;
using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Infrastructure.EF.HealthRecords.Queries;

internal static class Extensions
{
    public static AppointmentDetailsDto AsDetailsDto(this Appointment appointment)
        => new(
            appointment.AppointmentId,
            appointment.Title,
            appointment.Diagnosis,
            appointment.AppointmentDate,
            appointment.Notes,
            appointment.HealthRecordId
        );
}