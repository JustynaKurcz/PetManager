using PetManager.Core.Pets.Entities;
using PetManager.Core.Users.Enums;

namespace PetManager.Core.Users.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public DateTimeOffset? LastChangePasswordDate { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public UserRole Role { get; private set; }
    private HashSet<Pet> _pets = [];

    public IEnumerable<Pet> Pets
    {
        get => _pets;
        set => _pets = new HashSet<Pet>(value);
    }

    private User()
    {
    }

    private User(string email, string password, UserRole role)
    {
        Id = Guid.NewGuid();
        Email = email;
        Password = password;
        CreatedAt = DateTimeOffset.UtcNow;
        Role = role;
    }

    public static User Create(string email, string password, UserRole role)
        => new(email, password, role);

    public void ChangeInformation(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public void ChangePassword(string newPassword)
    {
        Password = newPassword;
        LastChangePasswordDate = DateTimeOffset.UtcNow;
    }
}