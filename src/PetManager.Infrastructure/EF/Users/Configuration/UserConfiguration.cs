using PetManager.Core.Users.Entities;

namespace PetManager.Infrastructure.EF.Users.Configuration;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.UserId);

        builder.Property<string>("FirstName")
            .IsRequired(false);

        builder.Property<string>("LastName")
            .IsRequired(false);

        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property<string>("Email")
            .IsRequired();

        builder.Property<string>("Password")
            .IsRequired();

        builder.Property<DateTimeOffset?>("LastChangePasswordDate");

        builder.Property<DateTime>("CreatedAt")
            .IsRequired();

        builder.Property<Guid>("RoleId")
            .IsRequired();

        builder.HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey("RoleId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Users");
    }
}