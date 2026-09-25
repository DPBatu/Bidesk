using Bidesk.Application.Repositories.Public.ProductionOperation;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;
using Bidesk.Persistence.Repositories;

namespace Bidesk.Persistence.Repositories.Public.ProductionOperation
{
    public class ProductionOperationReadRepository : ReadRepository<ProductionOperationEnt>, IProductionOperationReadRepository
    {
        public ProductionOperationReadRepository(BaseContext context) : base(context)
        {
        }
    }
}
