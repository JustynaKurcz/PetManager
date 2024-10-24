using PetManager.Application.HealthRecords.Commands.DeleteVaccinationToHealthRecord;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;

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
        var command = DeleteVaccinationToHealthRecordCommand();

        _healthRecordRepository
            .GetByIdAsync(command.HealthRecordId, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<HealthRecordNotFoundException>();
        exception.Message.ShouldBe($"Health record with ID {command.HealthRecordId} was not found.");

        await _healthRecordRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());

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
        var command = DeleteVaccinationToHealthRecordCommand();
        var healthRecord = HealthRecord.Create(Guid.NewGuid());

        _healthRecordRepository
            .GetByIdAsync(command.HealthRecordId, Arg.Any<CancellationToken>())
            .Returns(healthRecord);

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<VaccinationNotFoundException>();
        exception.Message.ShouldBe($"Vaccination with ID {command.VaccinationId} was not found.");

        await _healthRecordRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());

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
        var healthRecord = HealthRecord.Create(Guid.NewGuid());
        var vaccination = Vaccination.Create("Name", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(30),
            healthRecord.HealthRecordId);
        var command = DeleteVaccinationToHealthRecordCommand(healthRecord.HealthRecordId, vaccination.VaccinationId);

        healthRecord.AddVaccination(vaccination);

        _healthRecordRepository
            .GetByIdAsync(command.HealthRecordId, Arg.Any<CancellationToken>())
            .Returns(healthRecord);

        // Act
        await Act(command);

        // Assert
        await _healthRecordRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());

        await _healthRecordRepository
            .Received(1)
            .UpdateAsync(Arg.Any<HealthRecord>(), Arg.Any<CancellationToken>());

        await _healthRecordRepository
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private DeleteVaccinationToHealthRecordCommand DeleteVaccinationToHealthRecordCommand(Guid healthRecordId = default,
        Guid vaccinationId = default)
        => new(healthRecordId == default ? Guid.NewGuid() : healthRecordId,
            vaccinationId == default ? Guid.NewGuid() : vaccinationId);

    private readonly IHealthRecordRepository _healthRecordRepository;

    private readonly IRequestHandler<DeleteVaccinationToHealthRecordCommand> _handler;

    public DeleteVaccinationToHealthRecordCommandHandlerTests()
    {
        _healthRecordRepository = Substitute.For<IHealthRecordRepository>();

        _handler = new DeleteVaccinationToHealthRecordCommandHandler(_healthRecordRepository);
    }
}