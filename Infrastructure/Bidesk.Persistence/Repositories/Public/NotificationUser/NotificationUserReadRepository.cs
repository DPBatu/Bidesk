using Bidesk.Application.Repositories.Public.NotificationUser;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.Public.NotificationUser
{
    public class NotificationUserReadRepository : ReadRepository<NotificationUserEnt>, INotificationUserReadRepository
    {
        public NotificationUserReadRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
