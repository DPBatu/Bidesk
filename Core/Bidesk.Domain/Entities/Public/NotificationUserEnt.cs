namespace Bidesk.Domain.Entities.Public
{
    public class NotificationUserEnt : BaseEntity
    {
        public bool IsRead { get; set; } = false;
        public long UserId { get; set; }
        public AppUserEnt? User { get; set; }
        public long NotificationId { get; set; }
        public NotificationEnt? Notification { get; set; }
    }
}