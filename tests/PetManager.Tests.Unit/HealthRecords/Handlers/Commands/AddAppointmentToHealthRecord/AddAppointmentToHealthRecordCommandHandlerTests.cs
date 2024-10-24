using PetManager.Application.HealthRecords.Commands.AddAppointmentToHealthRecord;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;

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
        var command = CreateAddAppointmentToHealthRecordCommand();
        
        _healthRecordRepository
            .GetHealthRecordByIdAsync(command.HealthRecordId, Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<HealthRecordNotFoundException>();
        exception.Message.ShouldBe($"Health record with ID {command.HealthRecordId} was not found.");
        
        await _healthRecordRepository
            .Received(1)
            .GetHealthRecordByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        
        await _healthRecordRepository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<HealthRecord>(), Arg.Any<CancellationToken>());
        
        await _healthRecordRepository
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task given_valid_data_when_add_appointment_to_health_record_then_should_add_appointment_to_health_record()
    {
        // Arrange
        var command = CreateAddAppointmentToHealthRecordCommand();
        var healthRecord = HealthRecord.Create(Guid.NewGuid());
        
        _healthRecordRepository
            .GetHealthRecordByIdAsync(command.HealthRecordId, Arg.Any<CancellationToken>())
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
            .GetHealthRecordByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        
        await _healthRecordRepository
            .Received(1)
            .UpdateAsync(Arg.Any<HealthRecord>(), Arg.Any<CancellationToken>());
        
        await _healthRecordRepository
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private AddAppointmentToHealthRecordCommand CreateAddAppointmentToHealthRecordCommand()
        => new("TitleTest", "DiagnosisTest", DateTimeOffset.UtcNow, "NotesTest");

    private readonly IHealthRecordRepository _healthRecordRepository;

    private readonly IRequestHandler<AddAppointmentToHealthRecordCommand, AddAppointmentToHealthRecordResponse>
        _handler;

    public AddAppointmentToHealthRecordCommandHandlerTests()
    {
        _healthRecordRepository = Substitute.For<IHealthRecordRepository>();

        _handler = new AddAppointmentToHealthRecordCommandHandler(_healthRecordRepository);
    }
}