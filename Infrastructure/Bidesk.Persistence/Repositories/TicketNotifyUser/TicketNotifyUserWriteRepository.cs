using Bidesk.Application.Repositories.TicketNotifyUser;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TicketNotifyUser
{
    public class TicketNotifyUserWriteRepository : WriteRepository<TicketNotifyUserEnt>, ITicketNotifyUserWriteRepository
    {
        public TicketNotifyUserWriteRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
