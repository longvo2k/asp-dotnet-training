using StudyDotnet.Domain.Entities;
using StudyDotnet.Data.Repositories;

namespace StudyDotnet.Data.Uow;

public interface IUnitOfWork
{
    IRepository<Company, Guid> Companies { get; }
    IRepository<Device, Guid> Devices { get; }
    Task<int> SaveChangesAsync(string userName);
}
