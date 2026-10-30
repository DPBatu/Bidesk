using Bidesk.Application.Repositories.TicketGroup;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TicketGroup
{
    public class TicketGroupReadRepository : ReadRepository<TicketGroupEnt>, ITicketGroupReadRepository
    {
        public TicketGroupReadRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
