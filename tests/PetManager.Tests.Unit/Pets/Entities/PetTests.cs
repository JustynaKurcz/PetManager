using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Enums;

namespace PetManager.Tests.Unit.Pets.Entities;

public class PetTests
{
    [Theory]
    [InlineData("TestName", Species.Dog, "TestBreed", Gender.Male)]
    [InlineData("TestName2", Species.Hamster, "TestBreed2", Gender.Female)]
    public void given_pet_data_when_create_pet_then_should_create_pet(string name, Species species, string breed,
        Gender gender)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var birthDate = DateTimeOffset.UtcNow;

        // Act
        var pet = Pet.Create(name, species, breed, gender, birthDate, userId);

        // Assert
        pet.ShouldNotBeNull();
        pet.ShouldBeOfType<Pet>();
        pet.PetId.ShouldNotBe(Guid.Empty);
        pet.UserId.ShouldBe(userId);
    }
}