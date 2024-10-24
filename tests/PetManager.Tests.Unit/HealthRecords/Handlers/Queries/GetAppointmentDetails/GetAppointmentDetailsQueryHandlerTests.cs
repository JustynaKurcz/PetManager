using PetManager.Application.HealthRecords.Queries.GetAppointmentDetails;
using PetManager.Application.HealthRecords.Queries.GetAppointmentDetails.DTO;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Infrastructure.EF.HealthRecords.Queries.GetAppointmentDetails;

namespace PetManager.Tests.Unit.HealthRecords.Handlers.Queries.GetAppointmentDetails;

public sealed class GetAppointmentDetailsQueryHandlerTests
{
    private async Task<AppointmentDetailsDto> Act(GetAppointmentDetailsQuery query)
        => await _handler.Handle(query, CancellationToken.None);

    [Fact]
    public async Task
        given_invalid_healthRecord_id_when_get_appointment_details_then_should_throw_appointment_not_found_exception()
    {
        // Arrange
        var query = GetAppointmentDetailsQuery();
        _healthRecordRepository
            .GetByIdAsync(query.HealthRecordId, Arg.Any<CancellationToken>(), Arg.Any<bool>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => Act(query));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<HealthRecordNotFoundException>();
        exception.Message.ShouldBe($"Health record with ID {query.HealthRecordId} was not found.");

        await _healthRecordRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task
        given_valid_healthRecord_id_and_invalid_appointment_id_when_get_appointment_details_then_should_throw_appointment_not_found_exception()
    {
        // Arrange
        var query = GetAppointmentDetailsQuery();
        var healthRecord = HealthRecord.Create(Guid.NewGuid());
        _healthRecordRepository
            .GetByIdAsync(query.HealthRecordId, Arg.Any<CancellationToken>(), Arg.Any<bool>())
            .Returns(healthRecord);

        // Act
        var exception = await Record.ExceptionAsync(() => Act(query));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<AppointmentNotFoundException>();
        exception.Message.ShouldBe($"Appointment with ID {query.AppointmentId} was not found.");

        await _healthRecordRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task
        given_valid_healthRecord_id_and_valid_appointment_id_when_get_appointment_details_then_should_return_appointment_details()
    {
        // Arrange
        var healthRecord = HealthRecord.Create(Guid.NewGuid());
        var appointment =
            Appointment.Create("Title", "Diagnosis", DateTimeOffset.Now, "Notes", healthRecord.HealthRecordId);
        var query = GetAppointmentDetailsQuery(healthRecord.HealthRecordId, appointment.AppointmentId);
        healthRecord.AddAppointment(appointment);

        _healthRecordRepository
            .GetByIdAsync(query.HealthRecordId, Arg.Any<CancellationToken>(), Arg.Any<bool>())
            .Returns(healthRecord);

        // Act
        var response = await Act(query);

        // Assert
        response.ShouldNotBeNull();
        response.ShouldBeOfType<AppointmentDetailsDto>();
    }

    private GetAppointmentDetailsQuery GetAppointmentDetailsQuery(Guid healthRecordId = default,
        Guid appointmentId = default)
        => new(healthRecordId == default ? Guid.NewGuid() : healthRecordId,
            appointmentId == default ? Guid.NewGuid() : appointmentId);

    private readonly IHealthRecordRepository _healthRecordRepository;

    private IRequestHandler<GetAppointmentDetailsQuery, AppointmentDetailsDto> _handler;

    public GetAppointmentDetailsQueryHandlerTests()
    {
        _healthRecordRepository = Substitute.For<IHealthRecordRepository>();

        _handler = new GetAppointmentDetailsQueryHandler(_healthRecordRepository);
    }
}