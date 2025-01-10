using PetManager.Application.HealthRecords.Commands.DeleteVaccinationToHealthRecord;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Tests.Unit.HealthRecords.Factories;

namespace PetManager.Tests.Unit.HealthRecords.Handlers.Commands.DeleteVaccinationToHealthRecord;

public sealed class DeleteVaccinationToHealthRecordCommandHandlerTests
{
    private async Task Act(DeleteVaccinationToHealthRecordCommand command)
        => await _handler.Handle(command, CancellationToken.None);

    [Fact]
    public async Task
        given_invalid_health_record_id_when_delete_vaccination_to_health_record_then_should_throw_health_record_not_found_exception()
    {
        // Arrange
        var command = _healthRecordFactory.DeleteVaccinationToHealthRecordCommand();

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
        given_invalid_vaccination_id_when_delete_vaccination_to_health_record_then_should_throw_vaccination_not_found_exception()
    {
        // Arrange
        var command = _healthRecordFactory.DeleteVaccinationToHealthRecordCommand();
        var healthRecord = _healthRecordFactory.CreateHealthRecord();

        _healthRecordRepository
            .GetByIdAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>(),
                Arg.Any<bool>())
            .Returns(healthRecord);

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<VaccinationNotFoundException>();
        exception.Message.ShouldBe($"Vaccination with ID {command.VaccinationId} was not found.");

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
        given_valid_data_when_delete_vaccination_to_health_record_then_should_delete_vaccination_to_health_record()
    {
        // Arrange
        var healthRecord = _healthRecordFactory.CreateHealthRecord();
        var vaccination = _vaccinationFactory.CreateVaccination();
        var command =
            _healthRecordFactory.DeleteVaccinationToHealthRecordCommand(healthRecord.Id,
                vaccination.Id);

        healthRecord.AddVaccination(vaccination);

        _healthRecordRepository
            .GetByIdAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>(),
                Arg.Any<bool>())
            .Returns(healthRecord);

        // Act
        await Act(command);

        // Assert
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

    private readonly IRequestHandler<DeleteVaccinationToHealthRecordCommand> _handler;

    private readonly HealthRecordTestFactory _healthRecordFactory = new();
    private readonly VaccinationTestFactory _vaccinationFactory = new();


    public DeleteVaccinationToHealthRecordCommandHandlerTests()
    {
        _healthRecordRepository = Substitute.For<IHealthRecordRepository>();

        _handler = new DeleteVaccinationToHealthRecordCommandHandler(_healthRecordRepository);
    }
}