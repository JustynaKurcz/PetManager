using PetManager.Core.HealthRecords.Entitites;

namespace PetManager.Infrastructure.EF.HealthRecords.Configuration;

internal sealed class VaccinationConfiguration : IEntityTypeConfiguration<Vaccination>
{
    public void Configure(EntityTypeBuilder<Vaccination> builder)
    {
        builder.HasKey(x => x.VaccinationId);

        builder.Property<string>("VaccinationName")
            .IsRequired();

        builder.Property<DateTimeOffset>("VaccinationDate")
            .IsRequired();

        builder.Property<DateTimeOffset>("NextVaccinationDate")
            .IsRequired();

        builder.Property<Guid>("HealthRecordId")
            .IsRequired();

        builder.ToTable("Vaccinations");
    }
}