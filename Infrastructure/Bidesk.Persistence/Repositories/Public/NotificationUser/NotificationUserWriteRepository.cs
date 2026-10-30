using Bidesk.Application.Repositories.Public.NotificationUser;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;

namespace Bidesk.Persistence.Repositories.Public.NotificationUser
{
    public class NotificationUserWriteRepository : WriteRepository<NotificationUserEnt>, INotificationUserWriteRepository
    {
        public NotificationUserWriteRepository(BaseContext dbContext) : base(dbContext)
        { }
    }
}
