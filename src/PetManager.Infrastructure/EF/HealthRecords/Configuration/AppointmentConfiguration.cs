using PetManager.Core.HealthRecords.Entities;

namespace PetManager.Infrastructure.EF.HealthRecords.Configuration;

internal sealed class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property<string>("Title")
            .IsRequired();

        builder.Property<string>("Diagnosis")
            .IsRequired(false);

        builder.Property<DateTimeOffset>("AppointmentDate")
            .IsRequired();

        builder.Property<string>("Notes")
            .IsRequired(false);

        builder.Property<bool>("IsNotificationSent")
            .IsRequired();

        builder.Property<Guid>("HealthRecordId")
            .IsRequired();

        builder.ToTable("Appointments");
    }
}