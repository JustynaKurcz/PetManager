using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Enums;

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

        builder.Property<DateTimeOffset>("CreatedAt")
            .IsRequired();

        builder.Property<UserRole>("Role")
            .IsRequired();

        builder.ToTable("Users");
    }
}