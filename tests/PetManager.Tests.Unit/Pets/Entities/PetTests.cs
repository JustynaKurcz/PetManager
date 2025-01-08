using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Enums;

namespace PetManager.Tests.Unit.Pets.Entities;

public sealed class PetTests
{
    private static readonly Faker Faker = new();

    [Theory]
    [MemberData(nameof(GetValidPetData))]
    public void given_pet_data_when_create_pet_then_should_create_pet(string name, Species species, string breed,
        Gender gender, HealthRecord healthRecord)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var birthDate = DateTimeOffset.UtcNow;

        // Act
        var pet = Pet.Create(name, species, breed, gender, birthDate, userId, healthRecord);

        // Assert
        pet.ShouldNotBeNull();
        pet.ShouldBeOfType<Pet>();
        pet.PetId.ShouldNotBe(Guid.Empty);
        pet.UserId.ShouldBe(userId);
    }

    public static IEnumerable<object[]> GetValidPetData()
    {
        return Enumerable.Range(0, 5).Select(_ => (object[])
        [
            Faker.Name.FullName(),
            Faker.PickRandom<Species>(),
            Faker.Random.Word(),
            Faker.PickRandom<Gender>(),
            HealthRecord.Create()
        ]);
    }
}