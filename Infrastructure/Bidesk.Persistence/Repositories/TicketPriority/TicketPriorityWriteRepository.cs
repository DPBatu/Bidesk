using Bidesk.Application.Repositories.TicketPriority;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TicketPriority
{
    public class TicketPriorityWriteRepository : WriteRepository<TicketPriorityEnt>, ITicketPriorityWriteRepository
    {
        public TicketPriorityWriteRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
