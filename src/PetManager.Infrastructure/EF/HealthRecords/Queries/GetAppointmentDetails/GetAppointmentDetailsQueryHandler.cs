using PetManager.Application.HealthRecords.Queries.GetAppointmentDetails;
using PetManager.Application.HealthRecords.Queries.GetAppointmentDetails.DTO;
using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;

namespace PetManager.Infrastructure.EF.HealthRecords.Queries.GetAppointmentDetails;

internal sealed class GetAppointmentDetailsQueryHandler(IHealthRecordRepository healthRecordRepository)
    : IRequestHandler<GetAppointmentDetailsQuery, AppointmentDetailsDto>
{
    public async Task<AppointmentDetailsDto> Handle(GetAppointmentDetailsQuery query,
        CancellationToken cancellationToken)
    {
        var healthRecord =
            await healthRecordRepository.GetByIdAsync(query.HealthRecordId, cancellationToken, asNoTracking: true)
            ?? throw new HealthRecordNotFoundException(query.HealthRecordId);

        var appointment = healthRecord.Appointments.FirstOrDefault(a => a.AppointmentId == query.AppointmentId)
                          ?? throw new AppointmentNotFoundException(query.AppointmentId);

        return appointment.AsAppointmentDetailsDto();
    }
}