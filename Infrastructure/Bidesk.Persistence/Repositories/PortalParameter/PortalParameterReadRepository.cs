using Bidesk.Application.Repositories.PortalParameter;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.PortalParameter
{
    public class PortalParameterReadRepository : ReadRepository<PortalParameterEnt>, IPortalParameterReadRepository
    {
        public PortalParameterReadRepository(BaseContext dbContext) : base(dbContext)
        {
        }
    }
}
