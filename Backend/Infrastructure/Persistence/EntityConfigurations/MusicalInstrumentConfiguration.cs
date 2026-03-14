using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Common;

namespace Infrastructure.Persistence.EntityConfigurations;

public class MusicalInstrumentConfiguration : IEntityTypeConfiguration<MusicalInstrument>
{
    public void Configure(EntityTypeBuilder<MusicalInstrument> builder)
    {
        builder.HasKey(mi => mi.Id);

        builder.HasOne(i => i.Musician)
            .WithMany(m => m.Instruments)
            .HasForeignKey(i => i.MusicianId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(mi => mi.Name).HasColumnType("nvarchar(50)").IsRequired();
        builder.Property(mi => mi.Country).HasColumnType("nvarchar(50)").IsRequired(false);
        builder.Property(mi => mi.Description).HasColumnType("text").IsRequired(false);
        builder.Property(mi => mi.Type).HasConversion(
            t => t.ToString(),
            t => (InstrumentType)Enum.Parse(typeof(InstrumentType), t));
    }
}
