using Bidesk.Application.Repositories.TicketAttachment;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TicketAttachment
{
    public class TicketAttachmentReadRepository : ReadRepository<TicketAttachmentEnt>, ITicketAttachmentReadRepository
    {
        public TicketAttachmentReadRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
