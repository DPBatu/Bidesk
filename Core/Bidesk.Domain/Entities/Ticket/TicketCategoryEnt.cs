using System.ComponentModel.DataAnnotations;

namespace Bidesk.Domain.Entities.Ticket
{
    public class TicketCategoryEnt : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public ICollection<TicketSubCategoryEnt> SubCategories { get; set; } = new List<TicketSubCategoryEnt>();
    }
}
