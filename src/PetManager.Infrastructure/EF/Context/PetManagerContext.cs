using Microsoft.EntityFrameworkCore;

namespace PetManager.Infrastructure.EF.Context;

public class PetManagerContext : DbContext
{
    public PetManagerContext(DbContextOptions<PetManagerContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}