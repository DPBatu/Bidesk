using Bidesk.Application.Repositories.Public.WorkCenter;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;
using Bidesk.Persistence.Repositories;

namespace Bidesk.Persistence.Repositories.Public.WorkCenter
{
    public class WorkCenterReadRepository : ReadRepository<WorkCenterEnt>, IWorkCenterReadRepository
    {
        public WorkCenterReadRepository(BaseContext context) : base(context)
        {
        }
    }
}
