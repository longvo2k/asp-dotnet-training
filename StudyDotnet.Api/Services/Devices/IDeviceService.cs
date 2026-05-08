using StudyDotnet.Dtos;

namespace StudyDotnet.Services;

public interface IDeviceService
{
    Task<PagedResult<DeviceDto>> GetPaginatedListAsync(SearchDevicesRequest request);
    Task<PagedResult<DeviceDto>> SearchAsync(SearchDevicesRequest request);
}
