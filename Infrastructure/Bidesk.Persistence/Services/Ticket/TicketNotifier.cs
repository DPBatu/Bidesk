using Bidesk.Application.Abstraction.Services;
using Bidesk.Application.Abstraction.Services.User;
using Bidesk.Application.DTOs.User;
using Bidesk.Domain.Entities.Public;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Bidesk.Persistence.Services.Ticket
{
    /// <summary>
    /// Talep hareketlerinde mail + portal ici bildirim. Her yontem best-effort:
    /// gonderim hatasi ana islemi geri almaz, yalnizca loglanir.
    /// </summary>
    public class TicketNotifier
    {
        public const string PortalName = "bidesk";

        private readonly BaseContext _context;
        private readonly IMailService _mailService;
        private readonly INotificationService _notificationService;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<TicketNotifier> _logger;

        public TicketNotifier(BaseContext context, IMailService mailService, INotificationService notificationService,
            IConfiguration configuration, IHostEnvironment environment, ILogger<TicketNotifier> logger)
        {
            _context = context;
            _mailService = mailService;
            _notificationService = notificationService;
            _configuration = configuration;
            _environment = environment;
            _logger = logger;
        }

        public static string TicketUrl(long ticketId) => $"/bidesk/ticket/{ticketId}";

        /// <summary>Yeni talep: istek sahibine onay; teknisyene (yoksa grup teknisyenlerine) ve bildirim listesine bilgi.</summary>
        public async Task TicketCreatedAsync(TicketEnt ticket)
        {
            await SafeAsync(async () =>
            {
                var data = await LoadAsync(ticket.ID);
                if (data == null) return;

                await MailAsync(new[] { data.RequesterUser.Email },
                    $"Talebiniz alındı #{data.ID} - {data.Subject}",
                    Body(data, "Talebiniz bilgi işlem ekibine iletildi. Gelişmeler bu adrese bildirilecektir."));

                List<AppUserEnt> handlers = await HandlersAsync(data);
                var handlerMails = handlers.Select(h => h.Email).ToList();
                if (!string.IsNullOrWhiteSpace(data.Group.Email)) handlerMails.Add(data.Group.Email);

                await MailAsync(handlerMails,
                    $"Yeni talep #{data.ID} - {data.Subject}",
                    Body(data, data.TechnicianUserID.HasValue
                        ? "Size atanmış yeni bir talep var."
                        : $"{data.Group.Name} grubuna atanmamış yeni bir talep düştü."));

                await NotifyAsync(handlers.Select(h => h.Id), $"Yeni talep #{data.ID}", data.Subject, data.ID);

                await MailAsync(NotifyUserMails(data), $"Bilgilendirme: talep #{data.ID} - {data.Subject}",
                    Body(data, $"{data.RequesterUser.NameSurname} tarafından açılan talepte bilgilendirme listesine eklendiniz."));
            });
        }

        public async Task TechnicianAssignedAsync(long ticketId)
        {
            await SafeAsync(async () =>
            {
                var data = await LoadAsync(ticketId);
                if (data?.TechnicianUser == null) return;

                await MailAsync(new[] { data.TechnicianUser.Email }, $"Talep size atandı #{data.ID} - {data.Subject}",
                    Body(data, "Bu talep size atandı."));
                await NotifyAsync(new[] { data.TechnicianUser.Id }, $"Talep size atandı #{data.ID}", data.Subject, data.ID);
            });
        }

        /// <summary>Durum degisti: istek sahibi + bildirim listesi.</summary>
        public async Task StatusChangedAsync(long ticketId)
        {
            await SafeAsync(async () =>
            {
                var data = await LoadAsync(ticketId);
                if (data == null) return;

                string info = data.Status.IsResolved && !data.Status.IsClosed
                    ? "Talebiniz çözüldü olarak işaretlendi. Sorun devam ediyorsa talebe yanıt yazarak yeniden açabilirsiniz."
                    : $"Talebinizin durumu <b>{Encode(data.Status.Name)}</b> olarak güncellendi.";

                var mails = NotifyUserMails(data).Append(data.RequesterUser.Email);
                await MailAsync(mails, $"Talep durumu: {data.Status.Name} #{data.ID} - {data.Subject}", Body(data, info));
                await NotifyAsync(new[] { data.RequesterUserID }, $"Talep #{data.ID}: {data.Status.Name}", data.Subject, data.ID);
            });
        }

        /// <summary>Konusmaya yeni mesaj. Teknisyen yazdiysa istek sahibine, istek sahibi yazdiysa teknisyene gider.</summary>
        public async Task ConversationAddedAsync(long ticketId, TicketConversationEnt message, bool fromHandler)
        {
            if (message.IsInternal) return;

            await SafeAsync(async () =>
            {
                var data = await LoadAsync(ticketId);
                if (data == null) return;

                string body = Body(data, "Talebe yeni bir yanıt yazıldı:", message.Body);
                if (fromHandler)
                {
                    var mails = NotifyUserMails(data).Append(data.RequesterUser.Email);
                    await MailAsync(mails, $"Talebinize yanıt #{data.ID} - {data.Subject}", body);
                    await NotifyAsync(new[] { data.RequesterUserID }, $"Talebinize yanıt #{data.ID}", data.Subject, data.ID);
                }
                else
                {
                    var handlers = await HandlersAsync(data);
                    await MailAsync(handlers.Select(h => h.Email), $"İstek sahibi yanıtladı #{data.ID} - {data.Subject}", body);
                    await NotifyAsync(handlers.Select(h => h.Id), $"İstek sahibi yanıtladı #{data.ID}", data.Subject, data.ID);
                }
            });
        }

        public async Task OverdueAsync(long ticketId)
        {
            await SafeAsync(async () =>
            {
                var data = await LoadAsync(ticketId);
                if (data == null) return;

                var handlers = await HandlersAsync(data);
                await MailAsync(handlers.Select(h => h.Email), $"Hedef süresi geçti #{data.ID} - {data.Subject}",
                    Body(data, $"Talebin hedef çözüm tarihi ({data.DueDate:dd.MM.yyyy HH:mm}) geçti."));
                await NotifyAsync(handlers.Select(h => h.Id), $"Hedef süresi geçti #{data.ID}", data.Subject, data.ID);
            });
        }

        #region Helpers

        private Task<TicketEnt?> LoadAsync(long ticketId)
            => _context.Tickets.AsNoTracking()
                .Include(t => t.RequesterUser)
                .Include(t => t.TechnicianUser)
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include(t => t.Group)
                .Include(t => t.NotifyUsers.Where(n => !n.IsCanceled)).ThenInclude(n => n.User)
                .FirstOrDefaultAsync(t => t.ID == ticketId);

        /// <summary>Talebin sorumlusu; atanmamissa grubun tum teknisyenleri.</summary>
        private async Task<List<AppUserEnt>> HandlersAsync(TicketEnt data)
        {
            if (data.TechnicianUser != null) return new List<AppUserEnt> { data.TechnicianUser };

            return await _context.TechnicianGroups.AsNoTracking()
                .Where(t => t.GroupID == data.GroupID && !t.IsCanceled)
                .Select(t => t.User)
                .ToListAsync();
        }

        private static IEnumerable<string?> NotifyUserMails(TicketEnt data)
            => data.NotifyUsers.Select(n => n.User.Email);

        private async Task MailAsync(IEnumerable<string?> addresses, string subject, string body)
        {
            string[] to = addresses.Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
            if (to.Length == 0) return;
            await _mailService.SendMailAsync(to, subject, body);
        }

        private async Task NotifyAsync(IEnumerable<long> userIds, string title, string content, long ticketId)
        {
            long[] ids = userIds.Distinct().ToArray();
            if (ids.Length == 0) return;
            await _notificationService.AddAsync(new NotificationDto(title, content, ids, TicketUrl(ticketId), PortalName));
        }

        private string Body(TicketEnt data, string info, string? extraHtml = null)
        {
            string domain = _configuration[_environment.IsDevelopment() ? "Domain:DEV" : "Domain:PROD"] ?? string.Empty;
            string link = $"{domain.TrimEnd('/')}{TicketUrl(data.ID)}";

            return $@"<div style=""font-family:Segoe UI,Arial,sans-serif;font-size:14px"">
<p>{info}</p>
{(extraHtml == null ? string.Empty : $"<div style=\"border-left:3px solid #ccc;padding-left:8px;margin:8px 0\">{extraHtml}</div>")}
<table cellpadding=""4"" style=""border-collapse:collapse"">
<tr><td><b>Talep No</b></td><td>#{data.ID}</td></tr>
<tr><td><b>Konu</b></td><td>{Encode(data.Subject)}</td></tr>
<tr><td><b>İstek Sahibi</b></td><td>{Encode(data.RequesterUser.NameSurname)}</td></tr>
<tr><td><b>Grup</b></td><td>{Encode(data.Group.Name)}</td></tr>
<tr><td><b>Teknisyen</b></td><td>{Encode(data.TechnicianUser?.NameSurname ?? "Atanmamış")}</td></tr>
<tr><td><b>Durum</b></td><td>{Encode(data.Status.Name)}</td></tr>
<tr><td><b>Öncelik</b></td><td>{Encode(data.Priority.Name)}</td></tr>
</table>
<p><a href=""{link}"">Talebi portalda aç</a></p>
</div>";
        }

        private static string Encode(string? value) => WebUtility.HtmlEncode(value ?? string.Empty);

        private async Task SafeAsync(Func<Task> action)
        {
            try { await action(); }
            catch (Exception ex) { _logger.LogWarning(ex, "Talep bildirimi gönderilemedi."); }
        }

        #endregion
    }
}
