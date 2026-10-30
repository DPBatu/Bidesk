using Bidesk.Domain.Entities.Public;

namespace Bidesk.Domain.Entities.Ticket
{
    /// <summary>Teknisyen - grup iliskisi. Bir teknisyen birden fazla grupta olabilir.</summary>
    public class TechnicianGroupEnt : BaseEntity
    {
        public long GroupID { get; set; }
        public TicketGroupEnt Group { get; set; } = null!;

        public long UserID { get; set; }
        public AppUserEnt User { get; set; } = null!;
    }
}
