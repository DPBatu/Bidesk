using System.ComponentModel.DataAnnotations;

namespace Bidesk.Domain.Entities.Ticket
{
    public class TicketGroupEnt : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        /// <summary>Grup ortak posta kutusu; yeni talep bilgisi buraya da gider.</summary>
        [MaxLength(150)]
        public string? Email { get; set; }

        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<TechnicianGroupEnt> Technicians { get; set; } = new List<TechnicianGroupEnt>();
    }
}
