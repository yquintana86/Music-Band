using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public sealed class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.HasKey(pr => pr.Id);

        builder.Property(pr => pr.TokenHash).HasColumnType("NVARCHAR(500)").IsRequired();
        builder.Property(pr => pr.ExpiredUtc).IsRequired();
        builder.Property(pr => pr.UsedUtc).IsRequired(false);
        
        builder.HasOne(pr => pr.User)
            .WithMany();
        
        builder.Property(pr => pr.Used)
            .HasConversion(u => u ? 1 : 0, u => u == 1)
            .IsRequired();
    }
}
