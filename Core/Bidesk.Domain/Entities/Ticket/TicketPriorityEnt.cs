using System.ComponentModel.DataAnnotations;

namespace Bidesk.Domain.Entities.Ticket
{
    public class TicketPriorityEnt : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(20)]
        public string Severity { get; set; } = "secondary";

        public int SortOrder { get; set; }

        /// <summary>Oncelik secilmezse bu kayit atanir (Belirtilmemis).</summary>
        public bool IsDefault { get; set; }

        /// <summary>Hedef cozum suresi (saat). Bos ise SLA hedefi yok.</summary>
        public int? ResolveHours { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
