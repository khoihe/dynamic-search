namespace GenericRepository.Lib;

public abstract class GenericRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
{
    protected DbContext Context { get; private set; }

    public GenericRepository(DbContext context)
    {
        Context = context;
    }

    public virtual async Task<TEntity> AddAsync(TEntity e)
    {
        var Collection = Context.Set<TEntity>();
        await Collection.AddAsync(e);
        return e;
    }

    public virtual async Task<TEntity> UpdateAsync(TKey key, TEntity e)
    {
        var trackingEntity = await FindAsync(key);
        if (trackingEntity != null)
        {
            Update(e, trackingEntity);
        }
        return e;
    }

    public virtual Task<TEntity> FindAsync(TKey id)
    {
        return AsQueryable().FirstOrDefaultAsync(e => e.Id.Equals(id));
    }

    public virtual async Task<bool> RemoveAsync(TKey key)
    {
        var entity = await FindAsync(key);
        if (entity != null)
        {
            var Collection = Context.Set<TEntity>();
            Collection.Remove(entity);
            return true;
        }
        return false;
    }

    public virtual IQueryable<TEntity> AsQueryable()
    {
        var Collection = Context.Set<TEntity>();
        return Collection;
    }

    public virtual IQueryable<TEntity> AsFetchable()
    {
        return AsQueryable();
    }

    protected abstract void Update(TEntity requestObject, TEntity targetObject);
}