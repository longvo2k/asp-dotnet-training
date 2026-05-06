using Microsoft.EntityFrameworkCore;
using StudyDotnet.Api.Domain;
using StudyDotnet.Api.Dtos;
using StudyDotnet.Api.Repositories;
using StudyDotnet.Api.Tenancy;

namespace StudyDotnet.Api.Services;

public sealed class DeviceService : IDeviceService
{
    private readonly ITenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;

    public DeviceService(IUnitOfWork unitOfWork, ITenantContext tenantContext)
    {
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<PagedResult<DeviceDto>> SearchAsync(SearchDevicesRequest request)
    {
        var page = Math.Max(request.Page, 1);
        var pageSize = Math.Clamp(request.PageSize, 1, 50);
        var query = _unitOfWork.Devices.Query()
            .Include(device => device.Company)
            .Where(device => device.TenantId == _tenantContext.TenantId);

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            query = query.Where(device => device.Name.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase));
        }

        if (Enum.TryParse<DeviceSupplier>(request.Supplier, ignoreCase: true, out var supplier))
        {
            query = query.Where(device => device.Supplier == supplier);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(device => device.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(device => new DeviceDto(
                device.Id,
                device.Name,
                device.Supplier.ToString(),
                device.IsOnline,
                device.Company == null ? "Unknown" : device.Company.Name))
            .ToListAsync();

        return new PagedResult<DeviceDto>(items, page, pageSize, totalCount);
    }
}
