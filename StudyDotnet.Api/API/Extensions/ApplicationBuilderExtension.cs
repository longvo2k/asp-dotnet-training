using Microsoft.EntityFrameworkCore;
using StudyDotnet.Api.Middleware;
using StudyDotnet.Commons.Constants;
using StudyDotnet.Data.EF;

namespace StudyDotnet.Api.Extensions;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder UseStudyDotnetPipeline(this IApplicationBuilder app)
    {
        app.UseStudyDotnetSwagger();
        app.UseStudyDotnetDatabase();
        app.UseStudyDotnetErrorHandling();
        app.UseStudyDotnetSecurityPipeline();

        return app;
    }

    private static IApplicationBuilder UseStudyDotnetSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }

    private static IApplicationBuilder UseStudyDotnetDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StudyDbContext>();

        if (dbContext.Database.IsInMemory())
        {
            dbContext.Database.EnsureCreated();
            return app;
        }

        dbContext.Database.Migrate();
        return app;
    }

    private static IApplicationBuilder UseStudyDotnetErrorHandling(this IApplicationBuilder app)
    {
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

        return app;
    }

    private static IApplicationBuilder UseStudyDotnetSecurityPipeline(this IApplicationBuilder app)
    {
        app.UseMiddleware<ApiKeyMiddleware>();
        app.UseRouting();
        app.UseCors(CorsPolicies.AllowLocalFrontend);
        app.UseAuthentication();
        app.UseMiddleware<TenantMiddleware>();
        app.UseAuthorization();

        return app;
    }
}
