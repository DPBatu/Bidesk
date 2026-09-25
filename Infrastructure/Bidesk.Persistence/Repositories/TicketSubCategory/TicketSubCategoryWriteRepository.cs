using Bidesk.Application.Repositories.TicketSubCategory;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TicketSubCategory
{
    public class TicketSubCategoryWriteRepository : WriteRepository<TicketSubCategoryEnt>, ITicketSubCategoryWriteRepository
    {
        public TicketSubCategoryWriteRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
