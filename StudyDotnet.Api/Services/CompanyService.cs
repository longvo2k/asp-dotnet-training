using Microsoft.EntityFrameworkCore;
using StudyDotnet.Api.Dtos;
using StudyDotnet.Api.Repositories;
using StudyDotnet.Api.Tenancy;

namespace StudyDotnet.Api.Services;

public sealed class CompanyService : ICompanyService
{
    private readonly ITenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;

    public CompanyService(IUnitOfWork unitOfWork, ITenantContext tenantContext)
    {
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<IReadOnlyList<CompanyDto>> GetCompaniesAsync()
    {
        return await _unitOfWork.Companies.Query()
            .Where(company => company.TenantId == _tenantContext.TenantId)
            .OrderBy(company => company.Name)
            .Select(company => new CompanyDto(company.Id, company.Name, company.TaxCode))
            .ToListAsync();
    }

    public Task<int> CountCompaniesAsync()
    {
        return _unitOfWork.Companies.Query()
            .CountAsync(company => company.TenantId == _tenantContext.TenantId);
    }
}
