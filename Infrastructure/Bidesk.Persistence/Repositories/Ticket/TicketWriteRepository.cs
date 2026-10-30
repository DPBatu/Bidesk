using Bidesk.Application.Repositories.Ticket;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.Ticket
{
    public class TicketWriteRepository : WriteRepository<TicketEnt>, ITicketWriteRepository
    {
        public TicketWriteRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
