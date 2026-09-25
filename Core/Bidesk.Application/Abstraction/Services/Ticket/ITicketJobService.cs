namespace Bidesk.Application.Abstraction.Services.Ticket
{
    /// <summary>
    /// Hangfire'in gece/saatlik kostugu talep isleri.
    /// </summary>
    public interface ITicketJobService
    {
        /// <summary>
        /// "Cozuldu" durumunda parametredeki gun sayisindan uzun bekleyen talepleri
        /// "Tamamlandi" durumuna ceker.
        /// </summary>
        Task AutoCloseResolvedTicketsAsync();

        /// <summary>
        /// Hedef cozum tarihini gecen ve daha once uyari gitmemis talepler icin
        /// teknisyene / grup uyelerine gecikme maili atar.
        /// </summary>
        Task NotifyOverdueTicketsAsync();
    }
}
