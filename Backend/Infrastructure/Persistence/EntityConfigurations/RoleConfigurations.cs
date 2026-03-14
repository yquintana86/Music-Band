using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public sealed class RoleConfigurations : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name).HasColumnType("nvarchar(256)").IsRequired();
        builder.Property(r => r.NormalizedName).HasColumnType("nvarchar(256)").IsRequired(false);
        builder.Property(r => r.ConcurrencyStamp).HasColumnType("nvarchar(max)").IsRequired(false);

        builder.HasMany(r => r.Users)
                .WithMany(u => u.Roles)
                .UsingEntity("UserRoles");

        builder.HasMany(r => r.Permissions)
                .WithMany()
                .UsingEntity<RolePermission>();

        builder.HasIndex(r => r.Name).IsUnique();
    }
}
