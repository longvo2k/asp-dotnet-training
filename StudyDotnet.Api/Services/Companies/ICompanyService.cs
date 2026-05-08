using StudyDotnet.Dtos;

namespace StudyDotnet.Services;

public interface ICompanyService
{
    Task<IReadOnlyList<CompanyDto>> GetCompaniesAsync();
    Task<int> CountCompaniesAsync();
}
