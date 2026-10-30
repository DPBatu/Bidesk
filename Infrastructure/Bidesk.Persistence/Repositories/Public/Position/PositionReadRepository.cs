using Bidesk.Application.Repositories.Public.Position;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;
using Bidesk.Persistence.Repositories;

namespace Bidesk.Persistence.Repositories.Public.Position
{
    public class PositionReadRepository : ReadRepository<PositionEnt>, IPositionReadRepository
    {
        public PositionReadRepository(BaseContext context) : base(context)
        {
        }
    }
}
