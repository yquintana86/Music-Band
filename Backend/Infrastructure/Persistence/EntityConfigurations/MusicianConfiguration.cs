using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class MusicianConfiguration : IEntityTypeConfiguration<Musician>
{
    public void Configure(EntityTypeBuilder<Musician> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(m => m.Activities)
               .WithMany(a => a.Musicians)
               .UsingEntity<MusicianActivities>();

        builder.Property(x => x.FirstName).HasColumnType("nvarchar(25)").IsRequired();
        builder.Property(x => x.MiddleName).HasColumnType("nvarchar(25)").IsRequired(false);
        builder.Property(x => x.LastName).HasColumnType("nvarchar(50)").IsRequired();
        builder.Property(x => x.Age).IsRequired();
        builder.Property(x => x.BasicSalary).HasColumnType("decimal(8,2)").IsRequired();
        builder.Property(x => x.Experience).IsRequired();

        builder.ToTable(m => m.HasCheckConstraint("CK_Musician_Age", "Age > 0 AND Age < 150"));
        builder.ToTable(m => m.HasCheckConstraint("CK_Musician_Experience", "Experience > 0 AND Experience < 100"));
        builder.ToTable(m => m.HasCheckConstraint("CK_Basic_Salary", "BasicSalary > 0"));



    }
}
