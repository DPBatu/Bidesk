using System.ComponentModel.DataAnnotations;

namespace Bidesk.Domain.Entities.Ticket
{
    /// <summary>Talep durumu tanimi (Talep, Beklemede, Cozuldu...). Ekrandan cogaltilabilir.</summary>
    public class TicketStatusEnt : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        /// <summary>cc-tag severity: info, warn, success, danger, secondary, contrast.</summary>
        [MaxLength(20)]
        public string Severity { get; set; } = "info";

        public int SortOrder { get; set; }

        /// <summary>Yeni talep bu durumla acilir. Tek kayitta true olmali.</summary>
        public bool IsDefault { get; set; }

        /// <summary>Talep cozuldu sayilir; otomatik kapanis sayaci bu durumdan baslar.</summary>
        public bool IsResolved { get; set; }

        /// <summary>Talep kapali sayilir (Tamamlandi). Bekleyen listelerde gorunmez.</summary>
        public bool IsClosed { get; set; }

        /// <summary>Bu durumdayken SLA sayaci durur (Beklemede, Bilgi Istendi, Serviste...).</summary>
        public bool PausesSla { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
