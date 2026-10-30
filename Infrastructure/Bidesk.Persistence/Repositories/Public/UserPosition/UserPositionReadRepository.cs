using Bidesk.Application.Repositories.Public.UserPosition;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.Public.UserPosition
{
    public class UserPositionReadRepository : ReadRepository<UserPositionEnt>, IUserPositionReadRepository
    {
        public UserPositionReadRepository(BaseContext context) : base(context)
        {
        }
    }
}
