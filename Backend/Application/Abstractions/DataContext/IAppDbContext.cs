using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Abstractions.DataContext;

public interface IAppDbContext
{
    //Auth
    DbSet<User> Users { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<Permission> Permissions { get; set; }
    DbSet<RefreshToken> RefreshTokens { get; set; }

    //Business Logic
    DbSet<Domain.Entities.Musician> Musicians { get; set; }
    DbSet<Activity> Activities { get; set; }
    DbSet<MusicalInstrument> Instruments { get; set; }
    DbSet<MusicianActivities> MusicianActivities { get; set; }
    DbSet<MusicianPaymentDetail> MusicianPaymentDetails { get; set; }
    DbSet<RangePlus> RangePlus { get; set; }
    DatabaseFacade Database { get; }
}
