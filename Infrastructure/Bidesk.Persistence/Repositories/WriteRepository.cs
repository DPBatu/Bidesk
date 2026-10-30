using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Bidesk.Application.Repositories;
using Bidesk.Application.Features;
using Bidesk.Domain.Entities;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        private BaseContext _context;

        public WriteRepository(BaseContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<BaseRes> AddAsync(T model)
        {
            try
            {
                EntityEntry<T> entityEntry = await Table.AddAsync(model);
                if (entityEntry.State == EntityState.Added)
                {

                    return new BaseRes { IsSuccess = true, Id = model.ID, Message = "Kayıt Başarılı" };
                }
                else
                {
                    return new BaseRes { IsSuccess = false, Message = "! Beklenmeyen bir hata oluştu kayıt başarısız.", ErrorMessage = "! Beklenmeyen bir hata oluştu kayıt başarısız." };
                }
            }
            catch (Exception Ex)
            {

                return new BaseRes { IsSuccess = false, Message = "! Beklenmeyen bir hata oluştu kayıt başarısız.", ErrorMessage = Ex.Message + " " + Ex.InnerException?.Message };

            }
        }

        public async Task<List<T>> AddRangeAsync(List<T> datas)
        {            
            Task result = Table.AddRangeAsync(datas);
            await result;

            if (result.IsCompletedSuccessfully)
            {
                return datas;
            }
            return datas;
        }

        public bool Remove(T model)
        {
            model.IsCanceled = true;
            EntityEntry<T> entityEntry = Table.Update(model);
            return entityEntry.State == EntityState.Deleted;
        }

        public bool RemoveRange(List<T> datas)
        {
            datas.ForEach(data =>
            {
                data.IsCanceled = true;
            });

            Table.UpdateRange(datas);
            return true;
        }
        public async Task<BaseRes> RemoveAsync(long id)
        {
            T? model = await Table.FirstOrDefaultAsync(data => data.ID == id);
            if (model != null)
            {
                Remove(model);
                return new BaseRes { IsSuccess = true, Message = "Silme işlemi başarılı" };
            }
            else
            {
                return new BaseRes { IsSuccess = false, Message = "!Kayıt bulunamadı. Silme işlemi başarısız" };
            }
        }

        public BaseRes Update(T model)
        {
            EntityEntry<T> entityEntry = Table.Update(model);
            return new BaseRes { IsSuccess = entityEntry.State == EntityState.Modified };
        }

        public BaseRes UpdateRange(List<T> models)
        {
            Table.UpdateRange(models);
            return new BaseRes { IsSuccess = true, Message = "Başarılı" };
        }

        public async Task<BaseRes> SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return new BaseRes { IsSuccess = true, Message = "Başarılı" };
            }
            catch (Exception Ex)
            {

                return new BaseRes { ErrorMessage = Ex.Message + " " + Ex.InnerException?.Message, IsSuccess = false, Message = "!Kayıt İşlemi Başarısız" };
            }
        }
        public bool PermanentRemove(T model)
        {
            EntityEntry<T> entityEntry = Table.Remove(model);
            return entityEntry.State == EntityState.Deleted;
        }
    }
}
