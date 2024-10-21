using PetManager.Core.Users.Entities;
using PetManager.Infrastructure.EF.Context;

namespace PetManager.Infrastructure.EF.Users.Seeder;

public class RoleSeeder(PetManagerDbContext dbContext)
{
    public void Seed()
    {
        var roles = new[]
        {
            Role.Create("Admin"),
            Role.Create("User")
        };

        foreach (var role in roles)
        {
            if (!dbContext.Roles.Any(r => r.Name == role.Name))
            {
                dbContext.Roles.Add(role);
            }
        }

        dbContext.SaveChanges();

        // todo: haslo admina powinno byc zahashowane
        if (!dbContext.Users.Any(u => u.Email == "admin@petmanager.com"))
        {
            var role = dbContext.Roles.First(r => r.Name == "Admin");
            var adminUser = User.Create("admin@petmanager.com", "!23Haslo", role);
            dbContext.Users.Add(adminUser);
        }

        dbContext.SaveChanges();
    }
}