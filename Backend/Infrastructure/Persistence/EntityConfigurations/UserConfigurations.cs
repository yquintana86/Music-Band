using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public sealed class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Username).HasColumnType("nvarchar(256)").IsRequired();
        builder.Property(x => x.Email).HasColumnType("nvarchar(256)").IsRequired();
        builder.Property(x => x.FirstName).HasColumnType("nvarchar(50)").IsRequired();
        builder.Property(x => x.LastName).HasColumnType("nvarchar(200)").IsRequired(false);
        builder.Property(x => x.PasswordHash).HasColumnType("nvarchar(max)").IsRequired();
        
        builder.Property(x => x.EmailConfirmed).HasConversion(
            ec => ec ? 1 : 0,
            bec => bec == 1 ? true : false
            );

        builder.HasIndex(u => u.Username).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.IsLogged).HasConversion(
            t => t ? 1 : 0,
            b => b == 1 ? true : false
            );

    }
}
