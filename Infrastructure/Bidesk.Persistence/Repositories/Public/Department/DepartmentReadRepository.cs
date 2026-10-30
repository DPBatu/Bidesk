using Bidesk.Application.Repositories.Public.Department;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;
using Bidesk.Persistence.Repositories;

namespace Bidesk.Persistence.Repositories.Public.Department
{
    public class DepartmentReadRepository : ReadRepository<DepartmentEnt>, IDepartmentReadRepository
    {
        public DepartmentReadRepository(BaseContext context) : base(context)
        {
        }
    }
}
