using Bidesk.Domain.Entities.Public;
using System.ComponentModel.DataAnnotations;

namespace Bidesk.Domain.Entities.Ticket
{
    /// <summary>Bilgi islem talebi. ID talep numarasi olarak gosterilir.</summary>
    public class TicketEnt : BaseEntity
    {
        [MaxLength(250)]
        public string Subject { get; set; } = null!;

        /// <summary>Zengin metin (HTML).</summary>
        public string Description { get; set; } = null!;

        public long RequesterUserID { get; set; }
        public AppUserEnt RequesterUser { get; set; } = null!;

        public long StatusID { get; set; }
        public TicketStatusEnt Status { get; set; } = null!;

        public long PriorityID { get; set; }
        public TicketPriorityEnt Priority { get; set; } = null!;

        public long GroupID { get; set; }
        public TicketGroupEnt Group { get; set; } = null!;

        public long? TechnicianUserID { get; set; }
        public AppUserEnt? TechnicianUser { get; set; }

        public long? CategoryID { get; set; }
        public TicketCategoryEnt? Category { get; set; }

        public long? SubCategoryID { get; set; }
        public TicketSubCategoryEnt? SubCategory { get; set; }

        /// <summary>Hedef cozum tarihi (SLA). Beklemeli durumlarda ileri kaydirilir.</summary>
        public DateTime? DueDate { get; set; }

        /// <summary>SLA'yi durduran bir duruma gecilen an; geri donuldugunde DueDate bu kadar kaydirilir.</summary>
        public DateTime? SlaPausedAt { get; set; }

        /// <summary>Hedef suresi gectigi icin gecikme maili gonderildi mi.</summary>
        public bool OverdueNotified { get; set; }

        public DateTime? FirstResponseDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public DateTime? ClosedDate { get; set; }

        /// <summary>Cozumleme metni (HTML).</summary>
        public string? Resolution { get; set; }

        public ICollection<TicketNotifyUserEnt> NotifyUsers { get; set; } = new List<TicketNotifyUserEnt>();
        public ICollection<TicketConversationEnt> Conversations { get; set; } = new List<TicketConversationEnt>();
        public ICollection<TicketAttachmentEnt> Attachments { get; set; } = new List<TicketAttachmentEnt>();
    }
}
