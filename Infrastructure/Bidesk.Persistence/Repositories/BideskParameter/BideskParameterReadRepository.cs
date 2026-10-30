using Bidesk.Application.Repositories.BideskParameter;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.BideskParameter
{
    public class BideskParameterReadRepository : ReadRepository<BideskParameterEnt>, IBideskParameterReadRepository
    {
        public BideskParameterReadRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
