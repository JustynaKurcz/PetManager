using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails;
using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails.DTO;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Infrastructure.EF.HealthRecords.Queries.GetVaccinationDetails;

namespace PetManager.Tests.Unit.HealthRecords.Handlers.Queries.GetVaccinationDetails;

public sealed class GetVaccinationDetailsQueryHandlerTests
{
    private async Task<VaccinationDetailsDto> Act(GetVaccinationDetailsQuery query)
        => await _handler.Handle(query, CancellationToken.None);


    [Fact]
    public async Task
        given_invalid_healthRecord_id_when_get_vaccination_details_then_should_throw_health_record_not_found_exception()
    {
        // Arrange
        var query = GetVaccinationDetailsQuery();
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
        given_valid_healthRecord_id_and_invalid_vaccination_id_when_get_vaccination_details_then_should_throw_vaccination_not_found_exception()
    {
        // Arrange
        var query = GetVaccinationDetailsQuery();
        var healthRecord = HealthRecord.Create(Guid.NewGuid());
        _healthRecordRepository
            .GetByIdAsync(query.HealthRecordId, Arg.Any<CancellationToken>(), Arg.Any<bool>())
            .Returns(healthRecord);

        // Act
        var exception = await Record.ExceptionAsync(() => Act(query));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<VaccinationNotFoundException>();
        exception.Message.ShouldBe($"Vaccination with ID {query.VaccinationId} was not found.");

        await _healthRecordRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task
        given_valid_healthRecord_id_and_valid_vaccination_id_when_get_vaccination_details_then_should_return_vaccination_details()
    {
        // Arrange
        var healthRecord = HealthRecord.Create(Guid.NewGuid());
        var vaccination = Vaccination.Create("Name", DateTimeOffset.Now, DateTimeOffset.Now.AddDays(30),
            healthRecord.HealthRecordId);
        var query = GetVaccinationDetailsQuery(healthRecord.HealthRecordId, vaccination.VaccinationId);
        healthRecord.AddVaccination(vaccination);

        _healthRecordRepository
            .GetByIdAsync(query.HealthRecordId, Arg.Any<CancellationToken>(), Arg.Any<bool>())
            .Returns(healthRecord);

        // Act
        var response = await Act(query);

        // Assert
        response.ShouldNotBeNull();
        response.ShouldBeOfType<VaccinationDetailsDto>();
    }


    private GetVaccinationDetailsQuery GetVaccinationDetailsQuery(Guid healthRecordId = default,
        Guid vaccinationId = default)
        => new(healthRecordId == default ? Guid.NewGuid() : healthRecordId,
            vaccinationId == default ? Guid.NewGuid() : vaccinationId);

    private readonly IHealthRecordRepository _healthRecordRepository;

    private IRequestHandler<GetVaccinationDetailsQuery, VaccinationDetailsDto> _handler;

    public GetVaccinationDetailsQueryHandlerTests()
    {
        _healthRecordRepository = Substitute.For<IHealthRecordRepository>();

        _handler = new GetVaccinationDetailsQueryHandler(_healthRecordRepository);
    }
}