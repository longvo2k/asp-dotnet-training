using StudyDotnet.Api.Dtos;

namespace StudyDotnet.Api.Services;

public interface IDeviceService
{
    Task<PagedResult<DeviceDto>> GetPaginatedListAsync(SearchDevicesRequest request);
    Task<PagedResult<DeviceDto>> SearchAsync(SearchDevicesRequest request);
}
