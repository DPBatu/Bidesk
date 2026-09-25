using Bidesk.Application.Repositories.TicketConversation;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TicketConversation
{
    public class TicketConversationWriteRepository : WriteRepository<TicketConversationEnt>, ITicketConversationWriteRepository
    {
        public TicketConversationWriteRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
