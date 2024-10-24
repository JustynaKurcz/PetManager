using PetManager.Application.HealthRecords.Queries.GetAppointmentDetails.DTO;

namespace PetManager.Application.HealthRecords.Queries.GetAppointmentDetails;

public record GetAppointmentDetailsQuery(Guid HealthRecordId, Guid AppointmentId) : IRequest<AppointmentDetailsDto>;