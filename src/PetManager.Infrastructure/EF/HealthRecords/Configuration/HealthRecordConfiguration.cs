using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Infrastructure.EF.HealthRecords.Configuration;

internal sealed class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
{
    public void Configure(EntityTypeBuilder<HealthRecord> builder)
    {
        builder.HasKey(x => x.HealthRecordId);

        builder.Property<string>("Notes")
            .IsRequired(false);

        builder.Property<Guid>("PetId")
            .IsRequired();

        builder.HasMany(x => x.Appointments)
            .WithOne(x => x.HealthRecord)
            .HasForeignKey(x => x.HealthRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Vaccinations)
            .WithOne(x => x.HealthRecord)
            .HasForeignKey(x => x.HealthRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Pet)
            .WithOne(x => x.HealthRecord)
            .HasForeignKey<HealthRecord>(x => x.PetId);

        builder.ToTable("HealthRecords");
    }
}