using System.Text;
using StudyDotnet.Api.Dtos;

namespace StudyDotnet.Api.Services;

public sealed class AuthService : IAuthService
{
    public Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        if (request.UserName != "admin" || request.Password != "study")
        {
            return Task.FromResult<LoginResponse?>(null);
        }

        // This is intentionally not a real JWT. It is a readable placeholder for tracing auth flow.
        var token = Convert.ToBase64String(Encoding.UTF8.GetBytes("user=admin;tenant=demo"));
        return Task.FromResult<LoginResponse?>(new LoginResponse(token, "Bearer", "demo"));
    }
}
