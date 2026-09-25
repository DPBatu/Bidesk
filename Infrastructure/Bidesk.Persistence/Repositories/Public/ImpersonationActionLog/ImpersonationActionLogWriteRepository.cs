using Bidesk.Application.Repositories.Public.ImpersonationActionLog;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.Public.ImpersonationActionLog
{
    public class ImpersonationActionLogWriteRepository : WriteRepository<ImpersonationActionLogEnt>, IImpersonationActionLogWriteRepository
    {
        public ImpersonationActionLogWriteRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
