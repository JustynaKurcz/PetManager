using PetManager.Core.HealthRecords.Entitites;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Enums;

namespace PetManager.Infrastructure.EF.Pets.Configuration;

internal sealed class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.HasKey(x => x.PetId);

        builder.Property<string>("Name")
            .IsRequired();

        builder.Property<Species>("Species")
            .IsRequired();

        builder.Property<string>("Breed")
            .IsRequired();

        builder.Property<Gender>("Gender")
            .IsRequired();

        builder.Property<DateTimeOffset>("BirthDate")
            .IsRequired();

        builder.Property<Guid>("UserId")
            .IsRequired();

        builder.Property<Guid>("HealthRecordId")
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey("UserId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.HealthRecord)
            .WithOne(hr => hr.Pet)
            .HasForeignKey<HealthRecord>(hr => hr.PetId);

        builder.ToTable("Pets");
    }
}