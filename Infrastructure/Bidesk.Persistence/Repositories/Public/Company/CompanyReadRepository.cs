using Bidesk.Application.Repositories.Public.Company;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;
using Bidesk.Persistence.Repositories;

namespace Bidesk.Persistence.Repositories.Public.Company
{
    public class CompanyReadRepository : ReadRepository<CompanyEnt>, ICompanyReadRepository
    {
        public CompanyReadRepository(BaseContext context) : base(context)
        {
        }
    }
}
