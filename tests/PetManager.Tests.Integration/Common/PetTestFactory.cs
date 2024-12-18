using PetManager.Application.Pets.Commands.CreatePet;
using PetManager.Application.Pets.Queries.GetPetDetails;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Enums;

namespace PetManager.Tests.Integration.Common;

internal sealed class PetTestFactory
{
    private readonly Faker _faker = new();

    internal Pet CreatePet(Guid? userId)
        => Pet.Create(_faker.Person.FirstName, _faker.PickRandom<Species>(), _faker.Random.Word(),
            _faker.PickRandom<Gender>(), _faker.Date.PastOffset(10, DateTimeOffset.UtcNow),
            userId ?? _faker.Random.Guid());

    internal CreatePetCommand CreatePetCommand()
        => new(_faker.Person.FirstName, _faker.PickRandom<Species>(), _faker.Random.Word(), _faker.PickRandom<Gender>(),
            _faker.Date.PastOffset(10, DateTimeOffset.UtcNow));

    internal GetPetDetailsQuery GetPetDetailsQuery()
        => new(_faker.Random.Guid());

    public List<Pet> CreatePets(Guid? userId, int count)
        => Enumerable.Range(0, count)
            .Select(_ => CreatePet(userId))
            .ToList();
}