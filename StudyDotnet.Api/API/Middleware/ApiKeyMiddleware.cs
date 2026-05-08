using StudyDotnet.Commons.Constants;

namespace StudyDotnet.Api.Middleware;

public sealed class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/" || context.Request.Path.StartsWithSegments("/api/v2/auth"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(ApiHeaders.ApiKey, out var apiKey) || apiKey != DemoValues.ApiKey)
        {
            // Week 4: This is an explicit 401 Unauthorized status code.
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new
            {
                Error = $"Missing or invalid {ApiHeaders.ApiKey}",
                StatusCode = StatusCodes.Status401Unauthorized
            });
            return;
        }

        await _next(context);
    }
}
