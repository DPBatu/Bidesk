using Bidesk.Application.Repositories.TicketStatus;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TicketStatus
{
    public class TicketStatusWriteRepository : WriteRepository<TicketStatusEnt>, ITicketStatusWriteRepository
    {
        public TicketStatusWriteRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
