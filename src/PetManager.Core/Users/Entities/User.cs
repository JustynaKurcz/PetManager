using PetManager.Core.Users.Enums;

namespace PetManager.Core.Users.Entities;

public class User
{
    public Guid UserId { get; private set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTimeOffset? LastChangePasswordDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public UserRole Role { get; set; }

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