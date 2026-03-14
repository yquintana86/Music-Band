using Application.Abstractions.DataContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence.DataContext;

public sealed class AppDbContext : DbContext, IAppDbContext, IUnitOfWork
{

    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) :
        base(dbContextOptions) { }

    public DbSet<Musician> Musicians { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<MusicalInstrument> Instruments { get; set; }
    public DbSet<MusicianActivities> MusicianActivities { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<MusicianPaymentDetail> MusicianPaymentDetails { get; set ; }
    public DbSet<RangePlus> RangePlus { get ; set ; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        
        //Seed Data
        modelBuilder.SeedData();
    }
}
