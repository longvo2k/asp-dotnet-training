using StudyDotnet.Api.Dtos;

namespace StudyDotnet.Api.Services;

public interface ICompanyService
{
    Task<IReadOnlyList<CompanyDto>> GetCompaniesAsync();
    Task<int> CountCompaniesAsync();
}
