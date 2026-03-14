using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class MusicianActivitiesConfiguration : IEntityTypeConfiguration<MusicianActivities>
{
    public void Configure(EntityTypeBuilder<MusicianActivities> builder)
    {
        builder.HasKey(ma => new { ma.ActivityId, ma.MusicianId});

        builder.Property(ma => ma.SalaryByActivity).HasColumnType("decimal(6,2)");

        builder.ToTable(ma => ma.HasCheckConstraint("CK_Salary", "SalaryByActivity > 0"));

    }
}
