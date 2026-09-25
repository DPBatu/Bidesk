using Bidesk.Application.Abstraction.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bidesk.Persistence.Services
{
    public class UnitOfWorkService<TContext> : IUnitOfWorkService<TContext> where TContext : DbContext
    {
        public TContext Context { get; }
        private IDbContextTransaction _transaction;

        public UnitOfWorkService(TContext context)
        {
            Context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await Context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            await _transaction?.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            Context?.Dispose();
        }
    }
}
