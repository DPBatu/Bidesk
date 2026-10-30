using Bidesk.Application.Features;

namespace Bidesk.Application.Repositories
{
    public interface IWriteRepository<T>: IRepository<T> where T : class
    {
        Task<BaseRes> AddAsync(T model);
        Task<List<T>> AddRangeAsync(List<T> datas);
        bool Remove(T model);
        bool RemoveRange(List<T> datas);
        Task<BaseRes> RemoveAsync(long id);
        bool PermanentRemove(T model);
        BaseRes Update(T model);
        BaseRes UpdateRange(List<T> models);
        Task<BaseRes> SaveAsync();
    }
}
