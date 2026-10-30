using Bidesk.Application.Repositories.Public.Route;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;
using Bidesk.Persistence.Repositories;

namespace Bidesk.Persistence.Repositories.Public.Route
{
    public class RouteReadRepository : ReadRepository<RouteEnt>, IRouteReadRepository
    {
        public RouteReadRepository(BaseContext context) : base(context)
        {
        }
    }
}
