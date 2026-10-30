using Bidesk.Application.Repositories.Public.Endpoint;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;
using Bidesk.Persistence.Repositories;

namespace Bidesk.Persistence.Repositories.Public.Endpoint
{
    public class EndpointReadRepository : ReadRepository<EndpointEnt>, IEndpointReadRepository
    {
        public EndpointReadRepository(BaseContext context) : base(context)
        {
        }
    }
}
