using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using StudyDotnet.Commons.Constants;
using StudyDotnet.Commons.Tenancy;
using StudyDotnet.Data.EF;
using StudyDotnet.Data.Uow;
using StudyDotnet.Security.Auth;
using StudyDotnet.Services;

namespace StudyDotnet.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddStudyDotnetLayers(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        services.AddStudyDotnetSwagger();
        services.AddStudyDotnetSecurity();
        services.AddStudyDotnetCors();
        services.AddStudyDotnetDatabase(configuration);
        services.AddStudyDotnetApplicationServices();

        return services;
    }

    private static IServiceCollection AddStudyDotnetSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "Demo token",
                In = ParameterLocation.Header,
                Description = "Paste the token returned from POST /api/v2/auth/login."
            });

            options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Name = "X-Api-Key",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Description = "Use study-key for this demo."
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("Bearer", document, externalResource: null),
                    new List<string>()
                },
                {
                    new OpenApiSecuritySchemeReference("ApiKey", document, externalResource: null),
                    new List<string>()
                }
            });
        });

        return services;
    }

    private static IServiceCollection AddStudyDotnetSecurity(this IServiceCollection services)
    {
        services
            .AddAuthentication(DemoBearerAuthenticationHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, DemoBearerAuthenticationHandler>(
                DemoBearerAuthenticationHandler.SchemeName,
                options => { });
        services.AddAuthorization();

        return services;
    }

    private static IServiceCollection AddStudyDotnetCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicies.AllowLocalFrontend, policy =>
            {
                policy
                    .WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }

    private static IServiceCollection AddStudyDotnetDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<StudyDbContext>(options =>
        {
            if (UsePostgres(configuration))
            {
                options.UseNpgsql(configuration.GetConnectionString("Postgres"));
                return;
            }

            options.UseInMemoryDatabase("StudyDotnet");
        });

        return services;
    }

    private static IServiceCollection AddStudyDotnetApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITenantContext, TenantContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }

    private static bool UsePostgres(IConfiguration configuration)
    {
        return string.Equals(configuration["Database:Provider"], "Postgres", StringComparison.OrdinalIgnoreCase);
    }
}
