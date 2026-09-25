using Bidesk.Application.Repositories.TicketGroup;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TicketGroup
{
    public class TicketGroupWriteRepository : WriteRepository<TicketGroupEnt>, ITicketGroupWriteRepository
    {
        public TicketGroupWriteRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
