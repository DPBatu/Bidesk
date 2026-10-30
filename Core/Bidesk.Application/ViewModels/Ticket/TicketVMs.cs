namespace Bidesk.Application.ViewModels.Ticket
{
    /// <summary>Talep listeleri (Taleplerim, Bekleyen Istekler) satiri.</summary>
    public class TicketGridVM
    {
        public long ID { get; set; }
        public string Subject { get; set; } = string.Empty;
        public long RequesterUserID { get; set; }
        public string? RequesterName { get; set; }
        public long? TechnicianUserID { get; set; }
        public string? TechnicianName { get; set; }
        public long StatusID { get; set; }
        public string? StatusName { get; set; }
        public string? StatusSeverity { get; set; }
        public bool IsClosed { get; set; }
        public long PriorityID { get; set; }
        public string? PriorityName { get; set; }
        public string? PrioritySeverity { get; set; }
        public long GroupID { get; set; }
        public string? GroupName { get; set; }
        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsOverdue { get; set; }
        public bool HasAttachment { get; set; }
    }

    public class TicketAttachmentVM
    {
        public long? ID { get; set; }
        public long? TicketID { get; set; }
        public long? ConversationID { get; set; }
        public long? AttachmentID { get; set; }
        public string? Name { get; set; }
    }

    public class TicketConversationVM
    {
        public long ID { get; set; }
        public string Body { get; set; } = string.Empty;
        public bool IsInternal { get; set; }
        public long CreatedUserId { get; set; }
        public string? CreatedUserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<TicketAttachmentVM> Attachments { get; set; } = new();
    }

    public class TicketDetailVM : TicketGridVM
    {
        public string Description { get; set; } = string.Empty;
        public string? RequesterEmail { get; set; }
        public string? RequesterDepartment { get; set; }
        public string? RequesterPhone { get; set; }
        public long? CategoryID { get; set; }
        public long? SubCategoryID { get; set; }
        public string? Resolution { get; set; }
        public DateTime? FirstResponseDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<long> NotifyUserIDs { get; set; } = new();
        public List<TechnicianVM> NotifyUsers { get; set; } = new();
        public List<TicketConversationVM> Conversations { get; set; } = new();
        public List<TicketAttachmentVM> Attachments { get; set; } = new();

        /// <summary>Oturumdaki kullanici bu talebi yonetebilir mi (teknisyen / admin).</summary>
        public bool CanManage { get; set; }
    }

    public class TicketHistoryVM
    {
        public string FieldName { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedUserName { get; set; }
    }

    public class TicketCountVM
    {
        public long ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Severity { get; set; }
        public int Count { get; set; }
    }

    public class TicketDashboardVM
    {
        public int OpenCount { get; set; }
        public int UnassignedCount { get; set; }
        public int OverdueCount { get; set; }
        public int MyOpenCount { get; set; }
        public int ResolvedLast30Days { get; set; }
        public List<TicketCountVM> ByStatus { get; set; } = new();
        public List<TicketCountVM> ByGroup { get; set; } = new();
        public List<TicketCountVM> ByTechnician { get; set; } = new();
    }
}
