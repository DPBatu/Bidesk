using Bidesk.Application.Repositories.Public.UserDepartment;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;
using Bidesk.Persistence.Repositories;

namespace Bidesk.Persistence.Repositories.Public.UserDepartment
{
    public class UserDepartmentReadRepository : ReadRepository<UserDepartmentEnt>, IUserDepartmentReadRepository
    {
        public UserDepartmentReadRepository(BaseContext context) : base(context)
        {
        }
    }
}
