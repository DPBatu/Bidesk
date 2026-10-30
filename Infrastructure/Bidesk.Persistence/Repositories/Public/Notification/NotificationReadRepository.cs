using Bidesk.Application.Repositories.Public.Notification;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.Public.Notification
{
    public class NotificationReadRepository : ReadRepository<NotificationEnt>, INotificationReadRepository
    {
        public NotificationReadRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
