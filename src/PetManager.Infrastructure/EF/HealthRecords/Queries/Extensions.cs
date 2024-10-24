using PetManager.Application.HealthRecords.Queries.GetAppointmentDetails.DTO;
using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails.DTO;
using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Infrastructure.EF.HealthRecords.Queries;

internal static class Extensions
{
    public static AppointmentDetailsDto AsAppointmentDetailsDto(this Appointment appointment)
        => new(
            appointment.AppointmentId,
            appointment.Title,
            appointment.Diagnosis,
            appointment.AppointmentDate,
            appointment.Notes,
            appointment.HealthRecordId
        );

    public static VaccinationDetailsDto AsVaccinationDetailsDto(this Vaccination vaccination)
        => new(
            vaccination.VaccinationId,
            vaccination.VaccinationName,
            vaccination.VaccinationDate,
            vaccination.NextVaccinationDate,
            vaccination.HealthRecordId
        );
}