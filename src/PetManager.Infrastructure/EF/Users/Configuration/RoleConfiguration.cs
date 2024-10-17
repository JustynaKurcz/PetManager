using PetManager.Core.Users.Entities;

namespace PetManager.Infrastructure.EF.Users.Configuration;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.RoleId);

        builder.Property<string>("Name")
            .IsRequired();

        builder.ToTable("Roles");
    }
}