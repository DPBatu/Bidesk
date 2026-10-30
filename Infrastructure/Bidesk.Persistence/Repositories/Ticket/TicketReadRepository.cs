using Bidesk.Application.Repositories.Ticket;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.Ticket
{
    public class TicketReadRepository : ReadRepository<TicketEnt>, ITicketReadRepository
    {
        public TicketReadRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
