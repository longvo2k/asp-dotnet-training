using StudyDotnet.Api.Dtos;

namespace StudyDotnet.Api.Services;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}
