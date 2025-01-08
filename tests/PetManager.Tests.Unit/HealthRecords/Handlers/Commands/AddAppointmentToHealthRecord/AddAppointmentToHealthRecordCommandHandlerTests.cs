using PetManager.Application.HealthRecords.Commands.AddAppointmentToHealthRecord;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Tests.Unit.HealthRecords.Factories;

namespace PetManager.Tests.Unit.HealthRecords.Handlers.Commands.AddAppointmentToHealthRecord;

public sealed class AddAppointmentToHealthRecordCommandHandlerTests
{
    private async Task<AddAppointmentToHealthRecordResponse> Act(AddAppointmentToHealthRecordCommand command)
        => await _handler.Handle(command, CancellationToken.None);

    [Fact]
    public async Task
        given_invalid_health_record_id_when_add_appointment_to_health_record_then_should_throw_health_record_not_found_exception()
    {
        // Arrange
        var command = _healthRecordFactory.AddAppointmentToHealthRecordCommand();

        _healthRecordRepository
            .GetByIdAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>(),
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
            .GetByIdAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>(),
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
        given_valid_data_when_add_appointment_to_health_record_then_should_add_appointment_to_health_record()
    {
        // Arrange
        var command = _healthRecordFactory.AddAppointmentToHealthRecordCommand();
        var healthRecord = _healthRecordFactory.CreateHealthRecord();

        _healthRecordRepository
            .GetByIdAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>(),
                Arg.Any<bool>())
            .Returns(healthRecord);

        _healthRecordRepository
            .UpdateAsync(healthRecord, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _healthRecordRepository
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        var response = await Act(command);

        // Assert
        response.ShouldNotBeNull();
        response.ShouldBeOfType<AddAppointmentToHealthRecordResponse>();
        response.AppointmentId.ShouldNotBe(Guid.Empty);

        await _healthRecordRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>(),
                Arg.Any<bool>());

        await _healthRecordRepository
            .Received(1)
            .UpdateAsync(Arg.Any<HealthRecord>(), Arg.Any<CancellationToken>());

        await _healthRecordRepository
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private readonly IHealthRecordRepository _healthRecordRepository;

    private readonly IRequestHandler<AddAppointmentToHealthRecordCommand, AddAppointmentToHealthRecordResponse>
        _handler;

    private readonly HealthRecordTestFactory _healthRecordFactory = new();

    public AddAppointmentToHealthRecordCommandHandlerTests()
    {
        _healthRecordRepository = Substitute.For<IHealthRecordRepository>();
        _handler = new AddAppointmentToHealthRecordCommandHandler(_healthRecordRepository);
    }
}