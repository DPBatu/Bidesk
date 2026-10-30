namespace Bidesk.Domain.Entities.Ticket
{
    /// <summary>Talep konusmasi: istek sahibine yanit ya da yalniz teknisyenlerin gordugu not.</summary>
    public class TicketConversationEnt : BaseEntity
    {
        public long TicketID { get; set; }
        public TicketEnt Ticket { get; set; } = null!;

        /// <summary>HTML govde.</summary>
        public string Body { get; set; } = null!;

        /// <summary>true: ic not, istek sahibi gormez.</summary>
        public bool IsInternal { get; set; }
    }
}
