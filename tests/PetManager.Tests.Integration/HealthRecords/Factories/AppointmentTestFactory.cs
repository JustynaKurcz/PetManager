using PetManager.Application.HealthRecords.Queries.GetAppointmentDetails;
using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Tests.Integration.HealthRecords.Factories;

internal sealed class AppointmentTestFactory
{
    private readonly Faker _faker = new();

    internal Appointment CreateAppointment(Guid? healthRecordId)
        => Appointment.Create(_faker.Random.Word(), _faker.Random.Word(), _faker.Date.PastOffset().ToUniversalTime(),
            _faker.Random.Word(), healthRecordId ?? _faker.Random.Guid());

    internal GetAppointmentDetailsQuery GetAppointmentDetailsQuery()
        => new(_faker.Random.Guid(), _faker.Random.Guid());
}