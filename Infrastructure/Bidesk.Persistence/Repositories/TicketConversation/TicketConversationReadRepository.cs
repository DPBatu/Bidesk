using Bidesk.Application.Repositories.TicketConversation;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TicketConversation
{
    public class TicketConversationReadRepository : ReadRepository<TicketConversationEnt>, ITicketConversationReadRepository
    {
        public TicketConversationReadRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
