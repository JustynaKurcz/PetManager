using PetManager.Application.HealthRecords.Commands.AddAppointmentToHealthRecord;
using PetManager.Application.HealthRecords.Commands.AddVaccinationToHealthRecord;
using PetManager.Application.HealthRecords.Commands.DeleteAppointmentToHealthRecord;
using PetManager.Application.HealthRecords.Commands.DeleteVaccinationToHealthRecord;
using PetManager.Application.HealthRecords.Queries.GetHealthRecordDetails;
using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Tests.Integration.HealthRecords.Factories;

internal sealed class HealthRecordTestFactory
{
    private readonly Faker _faker = new();

    internal HealthRecord CreateHealthRecord(Guid? petId)
        => HealthRecord.Create(petId ?? _faker.Random.Guid());

    internal AddAppointmentToHealthRecordCommand AddAppointmentToHealthRecordCommand()
        => new(
            _faker.Random.Word(),
            _faker.Random.Word(),
            _faker.Date.PastOffset().ToUniversalTime(),
            _faker.Random.Word()
        );

    internal AddVaccinationToHealthRecordCommand AddVaccinationToHealthRecordCommand()
        => new(_faker.Random.Word(), _faker.Date.PastOffset().ToUniversalTime(), _faker.Date.Future().ToUniversalTime());

    internal DeleteAppointmentToHealthRecordCommand DeleteAppointmentToHealthRecordCommand()
        => new(_faker.Random.Guid(), _faker.Random.Guid());

    internal DeleteVaccinationToHealthRecordCommand DeleteVaccinationToHealthRecordCommand()
        => new(_faker.Random.Guid(), _faker.Random.Guid());

    public GetHealthRecordDetailsQuery GetHealthRecordDetailsQuery()
        => new(_faker.Random.Guid());
}