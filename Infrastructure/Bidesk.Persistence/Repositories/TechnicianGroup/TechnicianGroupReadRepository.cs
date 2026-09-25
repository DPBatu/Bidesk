using Bidesk.Application.Repositories.TechnicianGroup;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TechnicianGroup
{
    public class TechnicianGroupReadRepository : ReadRepository<TechnicianGroupEnt>, ITechnicianGroupReadRepository
    {
        public TechnicianGroupReadRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
