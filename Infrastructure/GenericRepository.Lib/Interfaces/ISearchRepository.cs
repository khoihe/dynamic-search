namespace GenericRepository.Lib.Interface;

public interface ISearchRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    IQueryable<TEntity> AsQueryable();
}