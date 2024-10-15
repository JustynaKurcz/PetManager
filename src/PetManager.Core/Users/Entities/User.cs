namespace PetManager.Core.Users.Entities;

public class User
{
    public Guid UserId { get; private set; }
    private string FirstName { get; set; }
    private string LastName { get; set; }
    public string Email { get; private set; }
    private string Password { get; set; }
    private DateTimeOffset? LastChangePasswordDate { get; set; }
    private DateTime CreatedAt { get; set; }
    
    public Guid RoleId { get; private set; }
    public virtual Role Role { get; private set; }

    private User()
    {
    }

    public User(string firstName, string lastName, string email, string password)
    {
        UserId = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        CreatedAt = DateTime.UtcNow;
    }

    public static User Create(string firstName, string lastName, string email, string password)
        => new(firstName, lastName, email, password);
}