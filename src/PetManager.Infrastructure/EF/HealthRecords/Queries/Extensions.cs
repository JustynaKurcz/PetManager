using PetManager.Application.HealthRecords.Queries.BrowseAppointments.DTO;
using PetManager.Application.HealthRecords.Queries.BrowseVaccinations.DTO;
using PetManager.Application.HealthRecords.Queries.GetAppointmentDetails.DTO;
using PetManager.Application.HealthRecords.Queries.GetHealthRecordDetails.DTO;
using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails.DTO;
using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Infrastructure.EF.HealthRecords.Queries;

internal static class Extensions
{
    public static AppointmentDetailsDto AsAppointmentDetailsDto(this Appointment appointment)
        => new(
            appointment.Id,
            appointment.Title,
            appointment.Diagnosis,
            appointment.AppointmentDate,
            appointment.Notes,
            appointment.HealthRecordId,
            appointment.IsNotificationSent
        );

    public static AppointmentDto AsAppointmentDto(this Appointment appointment)
        => new(
            appointment.Id,
            appointment.Title,
            appointment.AppointmentDate,
            appointment.IsNotificationSent
        );

    public static VaccinationDetailsDto AsVaccinationDetailsDto(this Vaccination vaccination)
        => new(
            vaccination.Id,
            vaccination.VaccinationName,
            vaccination.VaccinationDate,
            vaccination.NextVaccinationDate?.DateTime,
            vaccination.HealthRecordId,
            vaccination.IsNotificationSent
        );

    public static VaccinationDto AsVaccinationDto(this Vaccination vaccination)
        => new(
            vaccination.Id,
            vaccination.VaccinationName,
            vaccination.VaccinationDate,
            vaccination.NextVaccinationDate?.DateTime,
            vaccination.IsNotificationSent
        );

    public static HealthRecordDetailsDto AsDetailsDto(this HealthRecord healthRecord)
        => new(
            healthRecord.Id,
            healthRecord.PetId,
            healthRecord.Notes,
            healthRecord.Vaccinations.Select(v => v.AsVaccinationDetailsDto()).ToList(),
            healthRecord.Appointments.Select(a => a.AsAppointmentDetailsDto()).ToList()
        );
}