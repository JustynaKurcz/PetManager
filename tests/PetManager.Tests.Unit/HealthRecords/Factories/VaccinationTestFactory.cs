using Bogus;
using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails;
using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Tests.Unit.HealthRecords.Factories;

internal sealed class VaccinationTestFactory
{
    private readonly Faker _faker = new();

    internal Vaccination CreateVaccination()
        => Vaccination.Create(_faker.Random.Word(), _faker.Date.PastOffset(), _faker.Date.Future(),
            _faker.Random.Guid());

    internal GetVaccinationDetailsQuery GetVaccinationDetailsQuery(Guid healthRecordId = default,
        Guid vaccinationId = default)
        => new(healthRecordId == default ? _faker.Random.Guid() : healthRecordId,
            vaccinationId == default ? _faker.Random.Guid() : vaccinationId);
}