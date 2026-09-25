using Bidesk.Application.Repositories.Public.RoleRelation;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;
using Bidesk.Persistence.Repositories;

namespace Bidesk.Persistence.Repositories.Public.RoleRelation
{
    public class RoleRelationReadRepository : ReadRepository<RoleRelationEnt>, IRoleRelationReadRepository
    {
        public RoleRelationReadRepository(BaseContext context) : base(context)
        {
        }
    }
}
