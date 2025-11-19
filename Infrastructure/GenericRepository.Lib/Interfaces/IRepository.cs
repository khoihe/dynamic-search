namespace GenericRepository.Lib.Interface;

public interface IRepository<TEntity, TKey> : ISearchRepository<TEntity, TKey>, IFetchRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    Task<TEntity> AddAsync(TEntity e);
    Task<TEntity> UpdateAsync(TKey key, TEntity e);
    Task<bool> RemoveAsync(TKey key);
    Task<TEntity> FindAsync(TKey id);
}