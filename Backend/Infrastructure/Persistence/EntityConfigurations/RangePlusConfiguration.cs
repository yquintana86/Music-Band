using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public sealed class RangePlusConfiguration : IEntityTypeConfiguration<RangePlus>
{
    public void Configure(EntityTypeBuilder<RangePlus> builder)
    {
        builder.HasKey(x => x.Id);

        builder.ToTable(rp => rp.HasCheckConstraint("CHK_MinMax_Experience",
                                   "([MinExperience] >= 0 AND [MinExperience] < [MaxExperience])"));

        builder.Property(rp => rp.Plus).HasColumnType("decimal(4,2)");

    }
}
