using Bidesk.Application.Repositories.Public.DefectCode;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;
using Bidesk.Persistence.Repositories;

namespace Bidesk.Persistence.Repositories.Public.DefectCode
{
    public class DefectCodeReadRepository : ReadRepository<DefectCodeEnt>, IDefectCodeReadRepository
    {
        public DefectCodeReadRepository(BaseContext context) : base(context)
        {
        }
    }
}
