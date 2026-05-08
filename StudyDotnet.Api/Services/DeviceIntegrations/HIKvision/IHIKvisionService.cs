using StudyDotnet.Dtos;

namespace StudyDotnet.Services.DeviceIntegrations.HIKvision;

public interface IHIKvisionService
{
    Task<IReadOnlyList<DeviceUserDto>> PullUsersAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DeviceEventDto>> PullEventsAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task<DeviceCommandDto> BuildOpenDoorCommandAsync(string deviceSerialNumber, int doorNo);
}
