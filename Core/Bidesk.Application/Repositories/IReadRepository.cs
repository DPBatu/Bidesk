using System.Linq.Expressions;

namespace Bidesk.Application.Repositories
{
    public interface IReadRepository<T>: IRepository<T> where T : class
    {
        IQueryable<T> GetAll(bool includeCanceled = false, bool tracking = true, long companyId = 0);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool includeCanceled = false, bool tracking = true, long companyId = 0);
        Task<T?> GetSingleAsync(Expression<Func<T, bool>> method, bool includeCanceled = false, bool tracking = true, long companyId = 0);
        Task<T?> GetByIDAsync(long id, bool includeCanceled = false, bool tracking = true, long companyId = 0);
    }
}
