using PetManager.Application.Common.Context;
using PetManager.Application.Common.Pagination;
using PetManager.Application.HealthRecords.Queries.BrowseAppointments;
using PetManager.Application.HealthRecords.Queries.BrowseAppointments.DTO;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Exceptions;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Infrastructure.EF.HealthRecords.Queries.BrowseAppointments;
using PetManager.Tests.Unit.HealthRecords.Factories;

namespace PetManager.Tests.Unit.HealthRecords.Handlers.Queries.BrowseAppointments;

public sealed class BrowseAppointmentsQueryHandlerTests
{
    private async Task<PaginationResult<AppointmentDto>> Act(BrowseAppointmentsQuery query)
        => await _handler.Handle(query, CancellationToken.None);
    
    [Fact]
    public async Task given_valid_query_when_browse_appointments_then_should_return_appointments()
    {
        // Arrange
        var healthRecord = _healthRecordFactory.CreateHealthRecord();
        var query = _appointmentFactory.BrowseAppointmentsQuery(healthRecord.Id);
        var appointments = await _appointmentFactory.CreateAppointments();
        var userId = Guid.NewGuid();

        _healthRecordRepository
            .GetAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(healthRecord);

        _context.UserId.Returns(userId);

        _appointmentRepository.BrowseAsync(healthRecord.Id, userId, Arg.Any<CancellationToken>())
            .Returns(appointments);

        // Act
        var result = await Act(query);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(appointments.Count());
    }

    [Fact]
    public async Task given_valid_query_when_empty_appointments_then_should_return_empty_list()
    {
        // Arrange
        var healthRecord = _healthRecordFactory.CreateHealthRecord();
        var query = _appointmentFactory.BrowseAppointmentsQuery();
        var userId = Guid.NewGuid();

        _healthRecordRepository
            .GetAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(healthRecord);

        _context.UserId.Returns(userId);

        _appointmentRepository.BrowseAsync(healthRecord.Id, userId, Arg.Any<CancellationToken>())
            .Returns([]);

        // Act
        var result = await Act(query);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(0);
    }

    [Fact]
    public async Task
        given_health_record_not_found_when_browse_appointments_then_should_throw_health_record_not_found_exception()
    {
        // Arrange
        var query = _appointmentFactory.BrowseAppointmentsQuery();
        var userId = Guid.NewGuid();

        _healthRecordRepository
            .GetAsync(Arg.Any<Expression<Func<HealthRecord, bool>>>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        _context.UserId.Returns(userId);

        // Act & Assert
        await Should.ThrowAsync<HealthRecordNotFoundException>(async () => await Act(query));
    }

    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IHealthRecordRepository _healthRecordRepository;
    private readonly IContext _context;
    private readonly AppointmentTestFactory _appointmentFactory = new();
    private readonly HealthRecordTestFactory _healthRecordFactory = new();

    private readonly IRequestHandler<BrowseAppointmentsQuery, PaginationResult<AppointmentDto>> _handler;

    public BrowseAppointmentsQueryHandlerTests()
    {
        _context = Substitute.For<IContext>();
        _appointmentRepository = Substitute.For<IAppointmentRepository>();
        _healthRecordRepository = Substitute.For<IHealthRecordRepository>();

        _handler = new BrowseAppointmentsQueryHandler(_context, _appointmentRepository, _healthRecordRepository);
    }
}