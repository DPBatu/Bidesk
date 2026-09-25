using Bidesk.Application.Repositories.TechnicianGroup;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TechnicianGroup
{
    public class TechnicianGroupWriteRepository : WriteRepository<TechnicianGroupEnt>, ITechnicianGroupWriteRepository
    {
        public TechnicianGroupWriteRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
