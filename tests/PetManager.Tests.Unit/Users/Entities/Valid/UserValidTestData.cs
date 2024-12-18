using PetManager.Core.Users.Enums;

namespace PetManager.Tests.Unit.Users.Entities.Valid;

internal sealed class UserValidTestData : TheoryData<UserParams>
{
    private readonly Faker _faker = new();

    public UserValidTestData()
    {
        Add(new UserParams(
            _faker.Person.Email,
            _faker.Internet.Password(),
            _faker.PickRandom<UserRole>()
        ));
    }
}