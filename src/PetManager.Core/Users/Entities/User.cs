using PetManager.Core.Users.Enums;

namespace PetManager.Core.Users.Entities;

public class User
{
    public Guid UserId { get; private set; }
    private string FirstName { get; set; }
    private string LastName { get; set; }
    public string Email { get; private set; }
    public string Password { get; set; }
    private DateTimeOffset? LastChangePasswordDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public UserRole Role { get; private set; }

    private User()
    {
    }

    private User(string email, string password, UserRole role)
    {
        UserId = Guid.NewGuid();
        Email = email;
        Password = password;
        CreatedAt = DateTimeOffset.UtcNow;
        Role = role;
    }

    public static User Create(string email, string password, UserRole role)
        => new(email, password, role);
}