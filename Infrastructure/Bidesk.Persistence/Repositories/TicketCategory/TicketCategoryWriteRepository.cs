using Bidesk.Application.Repositories.TicketCategory;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TicketCategory
{
    public class TicketCategoryWriteRepository : WriteRepository<TicketCategoryEnt>, ITicketCategoryWriteRepository
    {
        public TicketCategoryWriteRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
