using StudyDotnet.Api.Domain;

namespace StudyDotnet.Api.Repositories;

public interface IUnitOfWork
{
    IRepository<Company, Guid> Companies { get; }
    IRepository<Device, Guid> Devices { get; }
    Task<int> SaveChangesAsync(string userName);
}
