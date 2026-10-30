using Bidesk.Application.Repositories.TicketNotifyUser;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TicketNotifyUser
{
    public class TicketNotifyUserReadRepository : ReadRepository<TicketNotifyUserEnt>, ITicketNotifyUserReadRepository
    {
        public TicketNotifyUserReadRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
