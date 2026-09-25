namespace Bidesk.Domain.Entities.Public
{
    public class NotificationEnt : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? PortalName { get; set; }
        public ICollection<NotificationUserEnt> Users { get; set; } = new List<NotificationUserEnt>();
    }
}
