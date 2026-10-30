using Microsoft.EntityFrameworkCore;

namespace Bidesk.Application.Repositories
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> Table { get; }
    }
}
