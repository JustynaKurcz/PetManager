using Microsoft.EntityFrameworkCore;
using PetManager.Core.Users.Entities;
using PetManager.Infrastructure.EF.Users.Configuration;

namespace PetManager.Infrastructure.EF.Context;

public class PetManagerDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public PetManagerDbContext(DbContextOptions<PetManagerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
    }
}