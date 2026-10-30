using Bidesk.Application.Repositories.Public.Menu;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;
using Bidesk.Persistence.Repositories;

namespace Bidesk.Persistence.Repositories.Public.Menu
{
    public class MenuReadRepository : ReadRepository<MenuEnt>, IMenuReadRepository
    {
        public MenuReadRepository(BaseContext context) : base(context)
        {
        }
    }
}
