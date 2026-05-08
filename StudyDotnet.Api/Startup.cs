using StudyDotnet.Api.Extensions;

namespace StudyDotnet.Api;

public sealed class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddStudyDotnetLayers(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseStudyDotnetPipeline();
        app.UseEndpoints(endpoints => endpoints.MapStudyDotnetEndpoints());
    }
}
