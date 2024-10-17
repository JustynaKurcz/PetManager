namespace PetManager.Core.Users.Entities;

public class Role
{
    public Guid RoleId { get; private set; }
    public string Name { get; set; }

    private Role()
    {
    }

    public Role(string name)
    {
        RoleId = Guid.NewGuid();
        Name = name;
    }

    public static Role Create(string name)
        => new(name);
}