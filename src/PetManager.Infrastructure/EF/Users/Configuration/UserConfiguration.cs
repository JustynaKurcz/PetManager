using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Enums;

namespace PetManager.Infrastructure.EF.Users.Configuration;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

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
        
        builder.HasMany(x =>x.Pets)
            .WithOne(x=>x.User)
            .HasForeignKey(x=>x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.ToTable("Users");
    }
}