using PetManager.Application.HealthRecords.Queries.BrowseAppointments;
using PetManager.Application.HealthRecords.Queries.GetAppointmentDetails;
using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Tests.Unit.HealthRecords.Factories;

internal sealed class AppointmentTestFactory
{
    private readonly Faker _faker = new();

    internal Appointment CreateAppointment(Guid healthRecordId = default)
        => Appointment.Create(_faker.Random.Word(), _faker.Random.Word(), _faker.Date.PastOffset(),
            _faker.Random.Word(), healthRecordId == default ? _faker.Random.Guid() : healthRecordId);

    internal GetAppointmentDetailsQuery GetAppointmentDetailsQuery(Guid healthRecordId = default,
        Guid appointmentId = default)
        => new(healthRecordId == default ? _faker.Random.Guid() : healthRecordId,
            appointmentId == default ? _faker.Random.Guid() : appointmentId);

    internal BrowseAppointmentsQuery BrowseAppointmentsQuery(Guid healthRecordId = default)
        => new()
        {
            HealthRecordId = healthRecordId == default ? _faker.Random.Guid() : healthRecordId,
            PageNumber = 1,
            PageSize = 25,
        };

    internal Task<IQueryable<Appointment>> CreateAppointments(int appointmentCount = 5)
        => Task.FromResult(Enumerable
            .Range(0, appointmentCount)
            .Select(_ => CreateAppointment())
            .AsQueryable()
        );
}