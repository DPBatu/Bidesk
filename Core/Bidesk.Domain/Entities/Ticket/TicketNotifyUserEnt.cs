using Bidesk.Domain.Entities.Public;

namespace Bidesk.Domain.Entities.Ticket
{
    /// <summary>"Bildirimde bulunulacaklar": talep hareketlerinde bilgi maili alan kullanicilar.</summary>
    public class TicketNotifyUserEnt : BaseEntity
    {
        public long TicketID { get; set; }
        public TicketEnt Ticket { get; set; } = null!;

        public long UserID { get; set; }
        public AppUserEnt User { get; set; } = null!;
    }
}
