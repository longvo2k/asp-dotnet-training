using StudyDotnet.Dtos;

namespace StudyDotnet.Services.DeviceIntegrations.ZKTeco;

public interface IZkTecoService
{
    Task<IReadOnlyList<DeviceEventDto>> ParsePushEventsAsync(string deviceSerialNumber, string rawBody);
    Task<DeviceCommandDto> BuildSyncTimeCommandAsync(string deviceSerialNumber);
    Task<DeviceCommandDto> BuildEnrollUserCommandAsync(string deviceSerialNumber, DeviceUserDto user);
}
