
namespace GenericRepository.Lib;

public abstract class BaseUnitOfWork : IUnitOfWork
{
    protected DbContext Context { get; private set; }
    private IDbContextTransaction _transaction;

    public BaseUnitOfWork(DbContext context)
    {
        Context = context;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await Context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        Context.SaveChanges();
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
        }
    }

    public async Task RollbackAsync()
    {
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
    }
}