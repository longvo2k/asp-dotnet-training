using Microsoft.AspNetCore.Mvc;
using StudyDotnet.Dtos;
using StudyDotnet.Security.Auth;

namespace StudyDotnet.Api.Controllers;

[ApiController]
[Route("api/v2/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);

        if (response is null)
        {
            return Unauthorized(new { Error = "Use admin/study for this demo." });
        }

        return Ok(response);
    }
}
