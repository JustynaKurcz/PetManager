using PetManager.Core.Pets.Entities;

namespace PetManager.Infrastructure.EF.Pets.Configuration;

internal sealed class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property<string>("FileName")
            .IsRequired();

        builder.Property<string>("BlobUrl")
            .IsRequired();

        builder.Property<DateTimeOffset>("UploadedAt")
            .IsRequired();

        builder.Property<Guid>("PetId")
            .IsRequired();

        builder.HasOne(x => x.Pet)
            .WithOne(i => i.Image)
            .HasForeignKey<Image>(x => x.PetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Images");
    }
}