using Microsoft.AspNetCore.Mvc;
using StudyDotnet.Services;

namespace StudyDotnet.Api.Controllers;

[ApiController]
[Route("api/v1/companies")]
public sealed class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await _companyService.GetCompaniesAsync();
        return Ok(companies);
    }

    [HttpGet("count")]
    public async Task<IActionResult> CountCompanies()
    {
        var count = await _companyService.CountCompaniesAsync();
        return Ok(new { Count = count });
    }
}
