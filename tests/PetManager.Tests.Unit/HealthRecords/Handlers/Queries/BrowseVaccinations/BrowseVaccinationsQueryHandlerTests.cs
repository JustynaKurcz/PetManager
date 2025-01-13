using PetManager.Application.Common.Context;
using PetManager.Application.Common.Pagination;
using PetManager.Application.HealthRecords.Queries.BrowseVaccinations;
using PetManager.Application.HealthRecords.Queries.BrowseVaccinations.DTO;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Infrastructure.EF.HealthRecords.Queries.BrowseVaccinations;
using PetManager.Tests.Unit.HealthRecords.Factories;

namespace PetManager.Tests.Unit.HealthRecords.Handlers.Queries.BrowseVaccinations;

public sealed class BrowseVaccinationsQueryHandlerTests
{
    private async Task<PaginationResult<VaccinationDto>> Act(BrowseVaccinationsQuery query)
        => await _handler.Handle(query, CancellationToken.None);

    [Fact]
    public async Task given_valid_query_when_browse_vaccinations_then_should_return_vaccinations()
    {
        // Arrange
        var healthRecord = _healthRecordFactory.CreateHealthRecord();
        var query = _vaccinationFactory.BrowseVaccinationsQuery();
        var vaccinations = await _vaccinationFactory.CreateVaccinations();
        var userId = Guid.NewGuid();

        _healthRecordRepository
            .GetAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(healthRecord);

        _context.UserId.Returns(userId);

        _vaccinationRepository.BrowseAsync(userId, Arg.Any<CancellationToken>())
            .Returns(vaccinations);

        // Act
        var result = await Act(query);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(vaccinations.Count());
    }

    [Fact]
    public async Task given_valid_query_when_empty_vaccinations_then_should_return_empty_list()
    {
        // Arrange
        var healthRecord = _healthRecordFactory.CreateHealthRecord();
        var query = _vaccinationFactory.BrowseVaccinationsQuery();
        var userId = Guid.NewGuid();

        _healthRecordRepository
            .GetAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(healthRecord);

        _context.UserId.Returns(userId);

        _vaccinationRepository.BrowseAsync(userId, Arg.Any<CancellationToken>())
            .Returns([]);

        // Act
        var result = await Act(query);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(0);
    }

    [Fact]
    public async Task
        given_health_record_not_found_when_browse_vaccinations_then_should_throw_health_record_not_found_exception()
    {
        // Arrange
        var query = _vaccinationFactory.BrowseVaccinationsQuery();
        var userId = Guid.NewGuid();

        _healthRecordRepository
            .GetAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        _context.UserId.Returns(userId);

        // Act & Assert
        await Should.ThrowAsync<HealthRecordNotFoundException>(async () => await Act(query));
    }

    private readonly IVaccinationRepository _vaccinationRepository;
    private readonly IHealthRecordRepository _healthRecordRepository;
    private readonly IContext _context;
    
    private readonly VaccinationTestFactory _vaccinationFactory = new();
    private readonly HealthRecordTestFactory _healthRecordFactory = new();

    private readonly IRequestHandler<BrowseVaccinationsQuery, PaginationResult<VaccinationDto>> _handler;

    public BrowseVaccinationsQueryHandlerTests()
    {
        _context = Substitute.For<IContext>();
        _vaccinationRepository = Substitute.For<IVaccinationRepository>();
        _healthRecordRepository = Substitute.For<IHealthRecordRepository>();

        _handler = new BrowseVaccinationsQueryHandler(_context, _vaccinationRepository, _healthRecordRepository);
    }
}