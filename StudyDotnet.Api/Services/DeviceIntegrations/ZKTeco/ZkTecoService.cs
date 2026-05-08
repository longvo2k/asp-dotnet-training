using System.Globalization;
using StudyDotnet.Dtos;

namespace StudyDotnet.Services.DeviceIntegrations.ZKTeco;

public sealed class ZkTecoService : IZkTecoService
{
    private readonly ILogger<ZkTecoService> _logger;

    public ZkTecoService(ILogger<ZkTecoService> logger)
    {
        _logger = logger;
    }

    public Task<IReadOnlyList<DeviceEventDto>> ParsePushEventsAsync(string deviceSerialNumber, string rawBody)
    {
        var events = rawBody
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(line => ParseEventLine(deviceSerialNumber, line))
            .Where(deviceEvent => deviceEvent is not null)
            .Cast<DeviceEventDto>()
            .ToList();

        _logger.LogInformation("Parsed {Count} ZKTeco events from {DeviceSerialNumber}.", events.Count, deviceSerialNumber);
        return Task.FromResult<IReadOnlyList<DeviceEventDto>>(events);
    }

    public Task<DeviceCommandDto> BuildSyncTimeCommandAsync(string deviceSerialNumber)
    {
        var command = new DeviceCommandDto(
            deviceSerialNumber,
            $"SET OPTION DateTime={DateTime.Now:yyyy-MM-dd HH:mm:ss}");

        return Task.FromResult(command);
    }

    public Task<DeviceCommandDto> BuildEnrollUserCommandAsync(string deviceSerialNumber, DeviceUserDto user)
    {
        var command = new DeviceCommandDto(
            deviceSerialNumber,
            $"DATA UPDATE USERINFO PIN={user.EmployeeCode}\tName={user.FullName}\tCard={user.CardNumber}");

        return Task.FromResult(command);
    }

    private static DeviceEventDto? ParseEventLine(string deviceSerialNumber, string line)
    {
        var parts = line.Split('\t', StringSplitOptions.TrimEntries);

        if (parts.Length < 2)
        {
            return null;
        }

        if (!DateTime.TryParseExact(
                parts[1],
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeLocal,
                out var eventTime))
        {
            return null;
        }

        return new DeviceEventDto(
            deviceSerialNumber,
            parts[0],
            eventTime,
            parts.Length >= 3 ? parts[2] : "Attendance");
    }
}
