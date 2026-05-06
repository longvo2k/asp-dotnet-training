using Microsoft.EntityFrameworkCore;
using StudyDotnet.Api.Data;
using StudyDotnet.Api.Domain;

namespace StudyDotnet.Api.Repositories;

public sealed class EfRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    private readonly DbSet<TEntity> _dbSet;

    public EfRepository(StudyDbContext dbContext)
    {
        _dbSet = dbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> Query()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public Task<int> CountAsync()
    {
        return _dbSet.CountAsync();
    }
}
