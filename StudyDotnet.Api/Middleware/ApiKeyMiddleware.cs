namespace StudyDotnet.Api.Middleware;

public sealed class ApiKeyMiddleware
{
    private const string HeaderName = "X-Api-Key";
    private const string ExpectedApiKey = "study-key";
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

        if (!context.Request.Headers.TryGetValue(HeaderName, out var apiKey) || apiKey != ExpectedApiKey)
        {
            // Week 4: This is an explicit 401 Unauthorized status code.
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new
            {
                Error = $"Missing or invalid {HeaderName}",
                StatusCode = StatusCodes.Status401Unauthorized
            });
            return;
        }

        await _next(context);
    }
}
