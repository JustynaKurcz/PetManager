using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails;
using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Tests.Integration.HealthRecords.Factories;

internal sealed class VaccinationTestFactory
{
    private readonly Faker _faker = new();

    internal Vaccination CreateVaccination(Guid? healthRecordId)
        => Vaccination.Create(_faker.Random.Word(), _faker.Date.PastOffset().ToUniversalTime(),
            _faker.Date.Future().ToUniversalTime(),
            healthRecordId ?? _faker.Random.Guid());

    internal GetVaccinationDetailsQuery GetVaccinationDetailsQuery()
        => new(_faker.Random.Guid(), _faker.Random.Guid());
}