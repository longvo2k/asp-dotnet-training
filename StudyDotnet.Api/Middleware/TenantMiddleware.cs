using System.Security.Claims;
using StudyDotnet.Api.Tenancy;

namespace StudyDotnet.Api.Middleware;

public sealed class TenantMiddleware
{
    private const string HeaderName = "X-Tenant";
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext)
    {
        if (!context.Request.Path.StartsWithSegments("/api/v1"))
        {
            await _next(context);
            return;
        }

        var tenantFromHeader = context.Request.Headers[HeaderName].FirstOrDefault();
        var tenantFromClaim = context.User.FindFirstValue("tenant");
        var tenantId = tenantFromHeader ?? tenantFromClaim;

        if (string.IsNullOrWhiteSpace(tenantId))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                Error = $"Missing tenant. Send {HeaderName} or authenticate with a tenant claim.",
                StatusCode = StatusCodes.Status400BadRequest
            });
            return;
        }

        tenantContext.SetTenant(tenantId);
        await _next(context);
    }
}
