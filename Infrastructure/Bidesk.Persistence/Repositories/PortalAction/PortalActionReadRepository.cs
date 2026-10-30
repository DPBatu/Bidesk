using Bidesk.Application.Repositories.PortalAction;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.PortalAction
{
    public class PortalActionReadRepository : ReadRepository<ActionEnt>, IPortalActionReadRepository
    {
        public PortalActionReadRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
