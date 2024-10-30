using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Users.Entities;
using PetManager.Infrastructure.EF.HealthRecords.Configuration;
using PetManager.Infrastructure.EF.Pets.Configuration;
using PetManager.Infrastructure.EF.Users.Configuration;

namespace PetManager.Infrastructure.EF.DbContext;

public class PetManagerDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<HealthRecord> HealthRecords { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Vaccination> Vaccinations { get; set; }

    public PetManagerDbContext(DbContextOptions<PetManagerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new PetConfiguration());
        modelBuilder.ApplyConfiguration(new HealthRecordConfiguration());
        modelBuilder.ApplyConfiguration(new AppointmentConfiguration());
        modelBuilder.ApplyConfiguration(new VaccinationConfiguration());
    }
}