using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Bidesk.Application.Repositories;
using Bidesk.Domain.Entities;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private long CompanyId => _context.CompanyId;
        private BaseContext _context;

        public ReadRepository(BaseContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll(bool includeCanceled = false, bool tracking = true, long companyId = 0)
        {
            var query = Table.AsQueryable();

            query = companyId == 0 ? query.Where(x => x.CompanyID == CompanyId) : query.Where(x => x.CompanyID == companyId);
            query = includeCanceled ? query : query.Where(x => !x.IsCanceled);

            if (!tracking)
                query = query.AsNoTracking();

            return query;
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool includeCanceled = false, bool tracking = true, long companyId = 0)
        {
            var query = GetAll(includeCanceled, tracking, companyId);
            return query.Where(method);
        }

        public Task<T?> GetSingleAsync(Expression<Func<T, bool>> method, bool includeCanceled = false, bool tracking = true, long companyId = 0)
        {
            var query = GetAll(includeCanceled, tracking, companyId);
            return query.SingleOrDefaultAsync(method);
        }

        public Task<T?> GetByIDAsync(long id, bool includeCanceled = false, bool tracking = true, long companyId = 0)
        {
            var query = GetAll(includeCanceled, tracking, companyId);
            return query.SingleOrDefaultAsync(x => x.ID == id);
        }
    }
}
