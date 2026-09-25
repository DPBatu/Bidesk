using Bidesk.Application.Repositories.Public.BusinessPartner;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;
using Bidesk.Persistence.Repositories;

namespace Bidesk.Persistence.Repositories.Public.BusinessPartner
{
    public class BusinessPartnerReadRepository : ReadRepository<BusinessPartnerEnt>, IBusinessPartnerReadRepository
    {
        public BusinessPartnerReadRepository(BaseContext context) : base(context)
        {
        }
    }
}
