using Bidesk.Application.Repositories.TicketSubCategory;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.TicketSubCategory
{
    public class TicketSubCategoryReadRepository : ReadRepository<TicketSubCategoryEnt>, ITicketSubCategoryReadRepository
    {
        public TicketSubCategoryReadRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
