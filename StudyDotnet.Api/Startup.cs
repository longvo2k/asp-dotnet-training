using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using StudyDotnet.Api.Auth;
using StudyDotnet.Api.Data;
using StudyDotnet.Api.Middleware;
using StudyDotnet.Api.Repositories;
using StudyDotnet.Api.Services;
using StudyDotnet.Api.Tenancy;

namespace StudyDotnet.Api;

public sealed class Startup
{
    private const string AllowLocalFrontendPolicy = "AllowLocalFrontend";

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Week 3: Controllers are discovered by ASP.NET Core and receive services by DI.
        services.AddControllers();
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

        // Week 4: Authentication reads the Bearer token and creates HttpContext.User.
        // Authorization makes [Authorize] attributes block anonymous requests.
        services
            .AddAuthentication(DemoBearerAuthenticationHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, DemoBearerAuthenticationHandler>(
                DemoBearerAuthenticationHandler.SchemeName,
                options => { });
        services.AddAuthorization();

        // Week 4: CORS controls which browser frontends can call this API.
        // This demo allows common local frontend ports only.
        services.AddCors(options =>
        {
            options.AddPolicy(AllowLocalFrontendPolicy, policy =>
            {
                policy
                    .WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        // Week 5: EF Core DbContext is scoped per request. This demo uses the EF in-memory
        // provider, but the repository/service code reads like a normal database-backed API.
        services.AddDbContext<StudyDbContext>(options =>
        {
            if (UsePostgres())
            {
                options.UseNpgsql(Configuration.GetConnectionString("Postgres"));
                return;
            }

            options.UseInMemoryDatabase("StudyDotnet");
        });
        services.AddScoped<ITenantContext, TenantContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IAuthService, AuthService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Week 3: Swagger/OpenAPI provides a browser UI for reading and testing endpoints.
        app.UseSwagger();
        app.UseSwaggerUI();

        SeedDatabase(app);

        // Week 4: Error handling turns unexpected exceptions into a predictable 500 response.
        // In a real project, log the exception and avoid returning internal details.
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new
                {
                    Error = "Unexpected server error",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            });
        });

        // Week 4: Status code pages give empty responses like 404 a small JSON body.
        app.UseStatusCodePages(async statusCodeContext =>
        {
            var response = statusCodeContext.HttpContext.Response;

            if (response.HasStarted || response.ContentType is not null)
            {
                return;
            }

            await response.WriteAsJsonAsync(new
            {
                Error = "Request failed",
                StatusCode = response.StatusCode
            });
        });

        // Week 4: Middleware runs before controllers. Try calling APIs with and without
        // the X-Api-Key header to see how a request can be stopped early.
        app.UseMiddleware<ApiKeyMiddleware>();
        app.UseRouting();
        app.UseCors(AllowLocalFrontendPolicy);
        app.UseAuthentication();
        app.UseMiddleware<TenantMiddleware>();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsJsonAsync(new
                {
                    App = "StudyDotnet.Api",
                    Try = new[]
                    {
                        "GET /api/v1/companies",
                        "GET /api/v1/companies/count",
                        "GET /api/v1/profile/me",
                        "POST /api/v1/devices",
                        "POST /api/v2/auth/login"
                    },
                    ApiKeyHeader = "X-Api-Key: study-key",
                    TenantHeader = "X-Tenant: demo",
                    AuthorizationHeader = "Authorization: Bearer <token from login>"
                });
            });

            endpoints.MapGet("/api/v1/debug/error", context =>
            {
                throw new InvalidOperationException("Demo exception for reading error handling flow.");
            });
        });
    }

    private static void SeedDatabase(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StudyDbContext>();

        if (dbContext.Database.IsInMemory())
        {
            dbContext.Database.EnsureCreated();
            return;
        }

        dbContext.Database.Migrate();
    }

    private bool UsePostgres()
    {
        return string.Equals(Configuration["Database:Provider"], "Postgres", StringComparison.OrdinalIgnoreCase);
    }
}
