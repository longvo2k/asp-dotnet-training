namespace StudyDotnet.Api.Extensions;

public static class EndpointRouteBuilderExtension
{
    public static IEndpointRouteBuilder MapStudyDotnetEndpoints(this IEndpointRouteBuilder endpoints)
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
                    "GET /api/v1/devices",
                    "POST /api/v1/devices/search",
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

        return endpoints;
    }
}
