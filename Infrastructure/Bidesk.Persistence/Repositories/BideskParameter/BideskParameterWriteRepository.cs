using Bidesk.Application.Repositories.BideskParameter;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.BideskParameter
{
    public class BideskParameterWriteRepository : WriteRepository<BideskParameterEnt>, IBideskParameterWriteRepository
    {
        public BideskParameterWriteRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
