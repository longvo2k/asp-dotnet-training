using StudyDotnet.Domain.Entities;
using StudyDotnet.Data.EF;
using StudyDotnet.Data.Repositories;

namespace StudyDotnet.Data.Uow;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly StudyDbContext _dbContext;

    public UnitOfWork(StudyDbContext dbContext)
    {
        _dbContext = dbContext;
        Companies = new EfRepository<Company, Guid>(dbContext);
        Devices = new EfRepository<Device, Guid>(dbContext);
    }

    public IRepository<Company, Guid> Companies { get; }
    public IRepository<Device, Guid> Devices { get; }

    public Task<int> SaveChangesAsync(string userName)
    {
        return _dbContext.SaveChangesAsync(userName);
    }
}
