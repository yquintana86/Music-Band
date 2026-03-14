using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public sealed class MusicianPaymentDetailConfiguration : IEntityTypeConfiguration<MusicianPaymentDetail>
{
    public void Configure(EntityTypeBuilder<MusicianPaymentDetail> builder)
    {

        builder.HasKey(mpd => mpd.Id);

        builder.HasOne(mpd => mpd.Musician)
            .WithMany(m => m.MusicianPaymentDetails)
            .IsRequired();

        builder.HasOne(mpd => mpd.RangePlus)
            .WithMany(m => m.MusicianPaymentDetails)
            .IsRequired();

        builder.Property(p => p.Salary)
            .HasColumnType("decimal(6,2)")
            .IsRequired();

        builder.Property(p => p.BasicSalary)
            .HasColumnType("decimal(6,2)")
            .IsRequired();

        builder.Property(p => p.PaymentDate)
            .HasColumnType("datetime2(3)")
            .IsRequired();
        
    }
}
