using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails;
using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails.DTO;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Infrastructure.EF.HealthRecords.Queries.GetVaccinationDetails;
using PetManager.Tests.Unit.HealthRecords.Factories;

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
        var query = _vaccinationFactory.GetVaccinationDetailsQuery();
        _healthRecordRepository
            .GetByIdAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>(),
                Arg.Any<bool>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => Act(query));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<HealthRecordNotFoundException>();
        exception.Message.ShouldBe($"Health record with ID {query.HealthRecordId} was not found.");

        await _healthRecordRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>(),
                Arg.Any<bool>());
    }

    [Fact]
    public async Task
        given_valid_healthRecord_id_and_invalid_vaccination_id_when_get_vaccination_details_then_should_throw_vaccination_not_found_exception()
    {
        // Arrange
        var query = _vaccinationFactory.GetVaccinationDetailsQuery();
        var healthRecord = _healthRecordFactory.CreateHealthRecord();
        _healthRecordRepository
            .GetByIdAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>(),
                Arg.Any<bool>())
            .Returns(healthRecord);

        // Act
        var exception = await Record.ExceptionAsync(() => Act(query));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<VaccinationNotFoundException>();
        exception.Message.ShouldBe($"Vaccination with ID {query.VaccinationId} was not found.");

        await _healthRecordRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>(),
                Arg.Any<bool>());
    }

    [Fact]
    public async Task
        given_valid_healthRecord_id_and_valid_vaccination_id_when_get_vaccination_details_then_should_return_vaccination_details()
    {
        // Arrange
        var healthRecord = _healthRecordFactory.CreateHealthRecord();
        var vaccination = _vaccinationFactory.CreateVaccination();
        var query = _vaccinationFactory.GetVaccinationDetailsQuery(healthRecord.Id,
            vaccination.Id);
        healthRecord.AddVaccination(vaccination);

        _healthRecordRepository
            .GetByIdAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>(),
                Arg.Any<bool>())
            .Returns(healthRecord);

        // Act
        var response = await Act(query);

        // Assert
        response.ShouldNotBeNull();
        response.ShouldBeOfType<VaccinationDetailsDto>();
    }

    private readonly IHealthRecordRepository _healthRecordRepository;

    private readonly IRequestHandler<GetVaccinationDetailsQuery, VaccinationDetailsDto> _handler;

    private readonly HealthRecordTestFactory _healthRecordFactory = new();
    private readonly VaccinationTestFactory _vaccinationFactory = new();

    public GetVaccinationDetailsQueryHandlerTests()
    {
        _healthRecordRepository = Substitute.For<IHealthRecordRepository>();

        _handler = new GetVaccinationDetailsQueryHandler(_healthRecordRepository);
    }
}