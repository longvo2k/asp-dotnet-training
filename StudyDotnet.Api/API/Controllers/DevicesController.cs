using Microsoft.AspNetCore.Mvc;
using StudyDotnet.Dtos;
using StudyDotnet.Services;

namespace StudyDotnet.Api.Controllers;

[ApiController]
[Route("api/v1/devices")]
public sealed class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDevices([FromQuery] SearchDevicesRequest request)
    {
        var result = await _deviceService.GetPaginatedListAsync(request);
        return Ok(result);
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchDevices(SearchDevicesRequest request)
    {
        var result = await _deviceService.SearchAsync(request);
        return Ok(result);
    }
}
