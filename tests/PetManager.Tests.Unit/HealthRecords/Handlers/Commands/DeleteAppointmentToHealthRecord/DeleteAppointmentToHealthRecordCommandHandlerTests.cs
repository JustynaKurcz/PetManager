using PetManager.Application.HealthRecords.Commands.DeleteAppointmentToHealthRecord;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Tests.Unit.HealthRecords.Factories;

namespace PetManager.Tests.Unit.HealthRecords.Handlers.Commands.DeleteAppointmentToHealthRecord;

public sealed class DeleteAppointmentToHealthRecordCommandHandlerTests
{
    private async Task Act(DeleteAppointmentToHealthRecordCommand command)
        => await _handler.Handle(command, CancellationToken.None);

    [Fact]
    public async Task
        given_invalid_health_record_id_when_delete_appointment_to_health_record_then_should_throw_health_record_not_found_exception()
    {
        // Arrange
        var command = _healthRecordFactory.DeleteAppointmentToHealthRecordCommand();

        _healthRecordRepository
            .GetAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>(),
                Arg.Any<bool>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<HealthRecordNotFoundException>();
        exception.Message.ShouldBe($"Health record with ID {command.HealthRecordId} was not found.");

        await _healthRecordRepository
            .Received(1)
            .GetAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>(),
                Arg.Any<bool>());

        await _healthRecordRepository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<HealthRecord>(), Arg.Any<CancellationToken>());

        await _healthRecordRepository
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task
        given_invalid_appointment_id_when_delete_appointment_to_health_record_then_should_throw_appointment_not_found_exception()
    {
        // Arrange
        var command = _healthRecordFactory.DeleteAppointmentToHealthRecordCommand();
        var healthRecord = _healthRecordFactory.CreateHealthRecord();

        _healthRecordRepository
            .GetAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>(),
                Arg.Any<bool>())
            .Returns(healthRecord);

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<AppointmentNotFoundException>();
        exception.Message.ShouldBe($"Appointment with ID {command.AppointmentId} was not found.");

        await _healthRecordRepository
            .Received(1)
            .GetAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>(),
                Arg.Any<bool>());

        await _healthRecordRepository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<HealthRecord>(), Arg.Any<CancellationToken>());

        await _healthRecordRepository
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task
        given_valid_data_when_delete_appointment_to_health_record_then_should_delete_appointment_to_health_record()
    {
        // Arrange
        var healthRecord = _healthRecordFactory.CreateHealthRecord();
        var appointment = _appointmentFactory.CreateAppointment();
        var command =
            _healthRecordFactory.DeleteAppointmentToHealthRecordCommand(healthRecord.Id,
                appointment.Id);

        healthRecord.AddAppointment(appointment);

        _healthRecordRepository
            .GetAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>(),
                Arg.Any<bool>())
            .Returns(healthRecord);

        _healthRecordRepository
            .UpdateAsync(healthRecord, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _healthRecordRepository
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        await Act(command);

        // Assert
        await _healthRecordRepository
            .Received(1)
            .GetAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>(),
                Arg.Any<bool>());

        await _healthRecordRepository
            .Received(1)
            .UpdateAsync(Arg.Any<HealthRecord>(), Arg.Any<CancellationToken>());

        await _healthRecordRepository
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private readonly IHealthRecordRepository _healthRecordRepository;

    private readonly IRequestHandler<DeleteAppointmentToHealthRecordCommand> _handler;

    private readonly HealthRecordTestFactory _healthRecordFactory = new();
    private readonly AppointmentTestFactory _appointmentFactory = new();

    public DeleteAppointmentToHealthRecordCommandHandlerTests()
    {
        _healthRecordRepository = Substitute.For<IHealthRecordRepository>();

        _handler = new DeleteAppointmentToHealthRecordCommandHandler(_healthRecordRepository);
    }
}