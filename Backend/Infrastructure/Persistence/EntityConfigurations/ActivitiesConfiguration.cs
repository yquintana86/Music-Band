using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class ActivitiesConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(a => a.Name).HasColumnType("nvarchar(50)").IsRequired();
        builder.Property(a => a.Client).HasColumnType("nvarchar(50)").IsRequired();

        builder.Property(a => a.Begin).IsRequired(false);
        builder.Property(a => a.End).IsRequired(false);
        builder.Property(a => a.Price).HasColumnType("decimal(8,2)");

        builder.Property(a => a.International)
            .HasConversion(a => a ? 1 : 0, a => a == 1 ? true : false)
            .IsRequired();
    }
}
