using Bidesk.Application.Repositories.Public.Material;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;
using Bidesk.Persistence.Repositories;

namespace Bidesk.Persistence.Repositories.Public.Material
{
    public class MaterialReadRepository : ReadRepository<MaterialEnt>, IMaterialReadRepository
    {
        public MaterialReadRepository(BaseContext context) : base(context)
        {
        }
    }
}
