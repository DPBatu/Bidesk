using System.ComponentModel.DataAnnotations;

namespace Bidesk.Domain.Entities.Ticket
{
    public class TicketAttachmentEnt : BaseEntity
    {
        public long TicketID { get; set; }
        public TicketEnt Ticket { get; set; } = null!;

        /// <summary>Yanita eklenen dosyada konusma kaydi; talebin kendisine eklenmisse bos.</summary>
        public long? ConversationID { get; set; }

        [MaxLength(200)]
        public string? Name { get; set; }
        public Guid? Keyword { get; set; }

        [MaxLength(200)]
        public string? AttachmentPath { get; set; }

        /// <summary>Portal dosya deposundaki kayit ID'si.</summary>
        public long AttachmentID { get; set; }
    }
}
