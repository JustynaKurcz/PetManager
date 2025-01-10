using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Infrastructure.EF.HealthRecords.Configuration;

internal sealed class VaccinationConfiguration : IEntityTypeConfiguration<Vaccination>
{
    public void Configure(EntityTypeBuilder<Vaccination> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property<string>("VaccinationName")
            .IsRequired();

        builder.Property<DateTimeOffset>("VaccinationDate")
            .IsRequired();

        builder.Property<DateTimeOffset>("NextVaccinationDate")
            .IsRequired();
        
        builder.Property<bool>("IsNotificationSent")
            .IsRequired();

        builder.Property<Guid>("HealthRecordId")
            .IsRequired();

        builder.ToTable("Vaccinations");
    }
}