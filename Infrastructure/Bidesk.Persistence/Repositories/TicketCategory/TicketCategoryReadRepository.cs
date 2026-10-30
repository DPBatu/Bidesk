using Bidesk.Application.Repositories.TicketCategory;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TicketCategory
{
    public class TicketCategoryReadRepository : ReadRepository<TicketCategoryEnt>, ITicketCategoryReadRepository
    {
        public TicketCategoryReadRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
