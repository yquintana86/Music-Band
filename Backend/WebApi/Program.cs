using Application;
using Infrastructure;
using Infrastructure.Persistence.DataContext;
using Presentation;
using Microsoft.EntityFrameworkCore;
using WebApi.GlobalExceptionHandler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Authentication;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.AddControllers()
     .AddApplicationPart(typeof(Presentation.MusicBandDependecyInjection).Assembly);


//builder.Services.AddScoped<ApiKeyAuthenticationFilter>(); -> this is the other one,
//in this one you register the service and included at controller level or at action method in controller level

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("MusicBandDatabase"));
});
    
builder.Services.AddApplication()
                .AddInfrastructure()
                .AddPresentation();

builder.Services.AddCors(options => { 
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.WithOrigins("http://localhost:4200",
                            "https://localhost:4200");
        
    });
});


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddOptions<EmailOptions>()
    .BindConfiguration("EmailSettings")
    .ValidateDataAnnotations()
    .Validate( o =>
         o.Port > 1 && 
         o.Port < 65535 &&
        !string.IsNullOrWhiteSpace(o.SmtpServer) && 
        !string.IsNullOrWhiteSpace(o.From) && 
        !string.IsNullOrWhiteSpace(o.Username) && 
        !string.IsNullOrWhiteSpace(o.Password),
        "Email configuration is invalid")
        .ValidateOnStart();


builder.Services.AddOptions<JwtOptions>()
    .BindConfiguration("Jwt")
    .ValidateDataAnnotations()
    .Validate(jwt => 
       !string.IsNullOrWhiteSpace(jwt.Issuer) &&
       !string.IsNullOrWhiteSpace(jwt.Audience) &&
       !string.IsNullOrWhiteSpace(jwt.SecretKey) &&
       jwt.ExpireUtc > 0 &&
       jwt.ExpireRefreshUtc > 0,
       "Jwt configuration is invalid")
    .ValidateOnStart();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
            ValidateIssuerSigningKey = true,

        };
    });


builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationHandler,PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseMiddleware<ApiKeyAuthenticationEndPointFilter>();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler();

//EndPoints
//app.AddInstrumentEndPoints();
//app.AddMusicianEndpoints();
//app.AddActivitiesEndPoints();

app.MapControllers();

app.Run();

