using Application.Abstractions.Authentication;
using Application.Abstractions.DataContext;
using Application.Abstractions.Email;
using Application.Abstractions.Repositories;
using Application.Abstractions.Utilities;
using Infrastructure.Authentication;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class MusicBandDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection app)
    {
        app.AddScoped<IInstrumentRepository,InstrumentRepository>();        
        app.AddScoped<IMusicianRepository, MusicianRepository>();
        app.AddScoped<IMusicianPaymentDetailsRepository, MusicianPaymentDetailsRepository>();
        app.AddScoped<IRangePlusRepository, RangePlusRepository>();
        app.AddScoped<IActivityRepository, ActivityRepository>();
        app.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        app.AddScoped<IRoleRepository, RoleRepository>();
        app.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        app.AddScoped<IJwtProvider, JwtProvider>();
        app.AddScoped<IPasswordHasher, PasswordHasher>();
        app.AddScoped<IPermissionService, PermissionService>();
        app.AddScoped<IEmailService, IEmailService>();
        app.AddScoped<IUrlService, IUrlService>();
        app.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<AppDbContext>());
        app.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        return app;
    }
}
