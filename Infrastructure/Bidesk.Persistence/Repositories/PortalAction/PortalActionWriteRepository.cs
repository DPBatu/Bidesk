using Bidesk.Application.Repositories.PortalAction;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.PortalAction
{
    public class PortalActionWriteRepository : WriteRepository<ActionEnt>, IPortalActionWriteRepository
    {
        public PortalActionWriteRepository(BaseContext dbContext) : base(dbContext)
        {
        }
    }
}
