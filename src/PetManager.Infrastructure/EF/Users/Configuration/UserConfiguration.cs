using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetManager.Core.Users.Entities;

namespace PetManager.Infrastructure.EF.Users.Configuration;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x=>x.UserId);
        
        builder.Property<string>("FirstName")
            .IsRequired();

        builder.Property<string>("LastName")
            .IsRequired();

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
        
        builder.ToTable("Users");
    }
}