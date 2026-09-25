using Bidesk.Application.Repositories.TicketAttachment;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TicketAttachment
{
    public class TicketAttachmentWriteRepository : WriteRepository<TicketAttachmentEnt>, ITicketAttachmentWriteRepository
    {
        public TicketAttachmentWriteRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
