using PetManager.Application.Pets.Commands.ChangePetInformation;
using PetManager.Application.Pets.Commands.CreatePet;
using PetManager.Application.Pets.Commands.DeletePet;
using PetManager.Application.Pets.Queries.GetPetDetails;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Enums;

namespace PetManager.Tests.Unit.Pets.Factories;

internal sealed class PetTestFactory
{
    private readonly Faker _faker = new();

    internal Pet CreatePet(Guid? userId = null)
        => Pet.Create(_faker.Person.FirstName, _faker.PickRandom<Species>(), _faker.Random.Word(),
            _faker.PickRandom<Gender>(), _faker.Date.PastOffset(10, DateTimeOffset.UtcNow),
            userId ?? _faker.Random.Guid(), HealthRecord.Create());

    internal CreatePetCommand CreatePetCommand()
        => new(_faker.Person.FirstName, _faker.PickRandom<Species>(), _faker.Random.Word(), _faker.PickRandom<Gender>(),
            _faker.Date.PastOffset(10, DateTimeOffset.UtcNow));

    internal ChangePetInformationCommand ChangePetInformationCommand()
        => new(_faker.PickRandom<Species>(), _faker.Random.Word(), _faker.PickRandom<Gender>());

    internal DeletePetCommand DeletePetCommand()
        => new(_faker.Random.Guid());

    internal GetPetDetailsQuery GetPetDetailsQuery()
        => new(_faker.Random.Guid());
}