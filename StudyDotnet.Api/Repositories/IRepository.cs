using StudyDotnet.Api.Domain;

namespace StudyDotnet.Api.Repositories;

public interface IRepository<TEntity, TKey>
    where TEntity : IEntity<TKey>
{
    IQueryable<TEntity> Query();
    Task<IReadOnlyList<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(TKey id);
    Task AddAsync(TEntity entity);
    Task<int> CountAsync();
}
