using Bogus;
using PetManager.Application.HealthRecords.Commands.AddAppointmentToHealthRecord;
using PetManager.Application.HealthRecords.Commands.AddVaccinationToHealthRecord;
using PetManager.Application.HealthRecords.Commands.DeleteAppointmentToHealthRecord;
using PetManager.Application.HealthRecords.Commands.DeleteVaccinationToHealthRecord;
using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Tests.Unit.HealthRecords.Factories;

internal sealed class HealthRecordTestFactory
{
    private readonly Faker _faker = new();

    internal HealthRecord CreateHealthRecord()
        => HealthRecord.Create(_faker.Random.Guid());

    internal AddAppointmentToHealthRecordCommand AddAppointmentToHealthRecordCommand()
        => new(_faker.Random.Word(), _faker.Random.Word(), _faker.Date.PastOffset(), _faker.Random.Word());

    internal AddVaccinationToHealthRecordCommand AddVaccinationToHealthRecordCommand()
        => new(_faker.Random.Word(), _faker.Date.PastOffset(), _faker.Date.Future());

    internal DeleteAppointmentToHealthRecordCommand DeleteAppointmentToHealthRecordCommand(
        Guid healthRecordId = default,
        Guid appointmentId = default)
        => new(healthRecordId == default ? _faker.Random.Guid() : healthRecordId,
            appointmentId == default ? _faker.Random.Guid() : appointmentId);

    internal DeleteVaccinationToHealthRecordCommand DeleteVaccinationToHealthRecordCommand(
        Guid healthRecordId = default,
        Guid vaccinationId = default)
        => new(healthRecordId == default ? _faker.Random.Guid() : healthRecordId,
            vaccinationId == default ? _faker.Random.Guid() : vaccinationId);
}