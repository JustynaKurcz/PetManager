namespace PetManager.Core.Users.Entities;

public class User
{
    public Guid UserId { get; private set; }
    private string FirstName { get; set; }
    private string LastName { get; set; }
    public string Email { get; private set; }
    public string Password { get; set; }
    private DateTimeOffset? LastChangePasswordDate { get; set; }
    private DateTime CreatedAt { get; set; }
    public Guid RoleId { get; private set; }
    public virtual Role Role { get; private set; }

    private User()
    {
    }

    private User(string email, string password, Guid roleId)
    {
        UserId = Guid.NewGuid();
        Email = email;
        Password = password;
        CreatedAt = DateTime.UtcNow;
        RoleId = roleId;
    }

    public static User Create(string email, string password, Guid roleId)
        => new(email, password, roleId);
}