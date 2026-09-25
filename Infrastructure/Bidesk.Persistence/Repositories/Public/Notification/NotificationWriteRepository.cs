using Bidesk.Application.Repositories.Public.Notification;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.Public.Notification
{
    public class NotificationWriteRepository : WriteRepository<NotificationEnt>, INotificationWriteRepository
    {
        public NotificationWriteRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
