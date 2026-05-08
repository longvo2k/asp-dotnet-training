using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyDotnet.Commons.Tenancy;

namespace StudyDotnet.Api.Controllers;

[ApiController]
[Route("api/v1/profile")]
[Authorize]
public sealed class ProfileController : ControllerBase
{
    private readonly ITenantContext _tenantContext;

    public ProfileController(ITenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }

    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        // Week 4: ControllerBase.User is a ClaimsPrincipal created by authentication middleware.
        ClaimsPrincipal currentUser = User;

        var userName = currentUser.Identity?.Name;
        var tenant = currentUser.FindFirstValue("tenant");

        return Ok(new
        {
            UserName = userName,
            Tenant = tenant,
            RequestTenant = _tenantContext.TenantId,
            IsAuthenticated = currentUser.Identity?.IsAuthenticated ?? false,
            Claims = currentUser.Claims.Select(claim => new
            {
                claim.Type,
                claim.Value
            })
        });
    }
}
