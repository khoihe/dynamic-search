namespace GenericRepository.Lib.Interface;

public interface IFetchRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    IQueryable<TEntity> AsFetchable();
}