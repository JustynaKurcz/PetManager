using Bogus;
using PetManager.Application.Pets.Commands.ChangePetInformation;
using PetManager.Application.Pets.Commands.CreatePet;
using PetManager.Application.Pets.Commands.DeletePet;
using PetManager.Application.Pets.Queries.GetPetDetails;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Enums;

namespace PetManager.Tests.Unit.Pets.Factories;

internal sealed class PetTestFactory
{
    private readonly Faker _faker = new();

    internal Pet CreatePet()
        => Pet.Create(_faker.Person.FirstName, _faker.PickRandom<Species>(), _faker.Random.Word(),
            _faker.PickRandom<Gender>(), _faker.Date.PastOffset(10, DateTimeOffset.UtcNow), _faker.Random.Guid());

    internal CreatePetCommand CreatePetCommand()
        => new(_faker.Person.FirstName, _faker.PickRandom<Species>(), _faker.Random.Word(), _faker.PickRandom<Gender>(),
            _faker.Date.PastOffset(10, DateTimeOffset.UtcNow));

    internal ChangePetInformationCommand ChangePetInformationCommand()
        => new(_faker.Person.FirstName, _faker.PickRandom<Species>(), _faker.Random.Word(), _faker.PickRandom<Gender>(),
            _faker.Date.PastOffset(10, DateTimeOffset.UtcNow));

    internal DeletePetCommand DeletePetCommand()
        => new(_faker.Random.Guid());
    
    internal GetPetDetailsQuery GetPetDetailsQuery()
        => new(_faker.Random.Guid());
}