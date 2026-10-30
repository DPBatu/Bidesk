namespace Bidesk.Application.ViewModels.User
{
    public class NotificationVm
    {
        public long? Id { get; set; } //NotificationUserId
        public long? NotificationId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Url { get; set; }
        public bool? IsRead { get; set; }
        public long? UserId { get; set; }
        public string? PortalName { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
