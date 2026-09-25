using Bidesk.Application.Repositories.Public.ProductionLine;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;
using Bidesk.Persistence.Repositories;

namespace Bidesk.Persistence.Repositories.Public.ProductionLine
{
    public class ProductionLineReadRepository : ReadRepository<ProductionLineEnt>, IProductionLineReadRepository
    {
        public ProductionLineReadRepository(BaseContext context) : base(context)
        {
        }
    }
}
