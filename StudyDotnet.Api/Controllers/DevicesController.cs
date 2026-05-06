using Microsoft.AspNetCore.Mvc;
using StudyDotnet.Api.Dtos;
using StudyDotnet.Api.Services;

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

    [HttpPost("search")]
    public async Task<IActionResult> SearchDevices(SearchDevicesRequest request)
    {
        var result = await _deviceService.SearchAsync(request);
        return Ok(result);
    }
}
