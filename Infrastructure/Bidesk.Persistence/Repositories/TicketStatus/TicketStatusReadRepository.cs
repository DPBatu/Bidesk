using Bidesk.Application.Repositories.TicketStatus;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TicketStatus
{
    public class TicketStatusReadRepository : ReadRepository<TicketStatusEnt>, ITicketStatusReadRepository
    {
        public TicketStatusReadRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
