using System.Net.Http.Json;
using StudyDotnet.Dtos;

namespace StudyDotnet.Services.DeviceIntegrations.HIKvision;

public sealed class HIKvisionService : IHIKvisionService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HIKvisionService> _logger;

    public HIKvisionService(HttpClient httpClient, ILogger<HIKvisionService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IReadOnlyList<DeviceUserDto>> PullUsersAsync(CancellationToken cancellationToken = default)
    {
        if (_httpClient.BaseAddress is null)
        {
            return DemoUsers();
        }

        var users = await _httpClient.GetFromJsonAsync<List<DeviceUserDto>>(
            "/ISAPI/AccessControl/UserInfo/Search",
            cancellationToken);

        return users ?? new List<DeviceUserDto>();
    }

    public async Task<IReadOnlyList<DeviceEventDto>> PullEventsAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default)
    {
        if (_httpClient.BaseAddress is null)
        {
            return DemoEvents(from);
        }

        var request = new
        {
            SearchDescription = "Study event query",
            BeginTime = from,
            EndTime = to
        };

        var response = await _httpClient.PostAsJsonAsync(
            "/ISAPI/AccessControl/AcsEvent",
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();
        _logger.LogInformation("Pulled HIKvision events from {From} to {To}.", from, to);

        return await response.Content.ReadFromJsonAsync<List<DeviceEventDto>>(cancellationToken)
            ?? new List<DeviceEventDto>();
    }

    public Task<DeviceCommandDto> BuildOpenDoorCommandAsync(string deviceSerialNumber, int doorNo)
    {
        var command = new DeviceCommandDto(
            deviceSerialNumber,
            $"PUT /ISAPI/AccessControl/RemoteControl/door/{doorNo}");

        return Task.FromResult(command);
    }

    private static IReadOnlyList<DeviceUserDto> DemoUsers()
    {
        return new List<DeviceUserDto>
        {
            new("E001", "Demo User", "10001"),
            new("E002", "Study User", "10002")
        };
    }

    private static IReadOnlyList<DeviceEventDto> DemoEvents(DateTime from)
    {
        return new List<DeviceEventDto>
        {
            new("HIK-DEMO-01", "E001", from.AddMinutes(5), "AccessGranted"),
            new("HIK-DEMO-01", "E002", from.AddMinutes(10), "AccessDenied")
        };
    }
}
