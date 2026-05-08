using StudyDotnet.Dtos;

namespace StudyDotnet.Security.Auth;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}
