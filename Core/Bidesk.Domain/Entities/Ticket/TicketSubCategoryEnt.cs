using System.ComponentModel.DataAnnotations;

namespace Bidesk.Domain.Entities.Ticket
{
    public class TicketSubCategoryEnt : BaseEntity
    {
        public long CategoryID { get; set; }
        public TicketCategoryEnt Category { get; set; } = null!;

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
