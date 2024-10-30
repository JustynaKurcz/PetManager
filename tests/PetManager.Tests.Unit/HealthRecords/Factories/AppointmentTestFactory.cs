using Bogus;
using PetManager.Application.HealthRecords.Queries.GetAppointmentDetails;
using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Tests.Unit.HealthRecords.Factories;

internal sealed class AppointmentTestFactory
{
    private readonly Faker _faker = new();

    internal Appointment CreateAppointment()
        => Appointment.Create(_faker.Random.Word(), _faker.Random.Word(), _faker.Date.PastOffset(),
            _faker.Random.Word(), _faker.Random.Guid());
    
    internal GetAppointmentDetailsQuery GetAppointmentDetailsQuery(Guid healthRecordId = default,
        Guid appointmentId = default)
        => new(healthRecordId == default ? _faker.Random.Guid() : healthRecordId,
            appointmentId == default ? _faker.Random.Guid() : appointmentId);
}