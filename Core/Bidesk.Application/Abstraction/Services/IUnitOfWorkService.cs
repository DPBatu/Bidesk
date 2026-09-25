using Microsoft.EntityFrameworkCore;

namespace Bidesk.Application.Abstraction.Services
{
    public interface IUnitOfWorkService<TContext> : IDisposable where TContext : DbContext
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<int> SaveChangesAsync();

        TContext Context { get; }
    }
}
