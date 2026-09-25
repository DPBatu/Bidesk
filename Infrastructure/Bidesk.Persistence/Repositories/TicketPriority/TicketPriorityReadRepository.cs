using Bidesk.Application.Repositories.TicketPriority;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TicketPriority
{
    public class TicketPriorityReadRepository : ReadRepository<TicketPriorityEnt>, ITicketPriorityReadRepository
    {
        public TicketPriorityReadRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
