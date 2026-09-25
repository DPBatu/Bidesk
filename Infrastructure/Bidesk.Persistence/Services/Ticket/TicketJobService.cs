using Bidesk.Application.Abstraction.Services.Ticket;
using Bidesk.Domain.Entities;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bidesk.Persistence.Services.Ticket
{
    /// <summary>
    /// Hangfire isleri. HttpContext olmadigi icin ContextService varsayilan sirket/kullaniciyi doner.
    /// </summary>
    public class TicketJobService : ITicketJobService
    {
        private readonly BaseContext _context;
        private readonly TicketNotifier _notifier;
        private readonly ILogger<TicketJobService> _logger;

        public TicketJobService(BaseContext context, TicketNotifier notifier, ILogger<TicketJobService> logger)
        {
            _context = context;
            _notifier = notifier;
            _logger = logger;
        }

        public async Task AutoCloseResolvedTicketsAsync()
        {
            long companyId = _context.CompanyId;
            string? raw = await _context.BideskParameters
                .Where(p => p.CompanyID == companyId && !p.IsCanceled && p.ParamName == TicketDefinitionService.AutoCloseDaysParam)
                .Select(p => p.ParamValue)
                .FirstOrDefaultAsync();

            // 0 ya da tanimsiz: otomatik kapanis kapali.
            if (!int.TryParse(raw, out int days) || days <= 0) return;

            var closed = await _context.TicketStatuses
                .Where(s => s.CompanyID == companyId && !s.IsCanceled && s.IsActive && s.IsClosed)
                .OrderBy(s => s.SortOrder)
                .FirstOrDefaultAsync();
            if (closed == null)
            {
                _logger.LogWarning("Otomatik kapanış atlandı: kapalı tipte durum tanımlı değil.");
                return;
            }

            DateTime now = DateTime.Now;
            DateTime limit = now.AddDays(-days);
            var tickets = await _context.Tickets
                .Include(t => t.Status)
                .Where(t => t.CompanyID == companyId && !t.IsCanceled
                    && t.Status.IsResolved && !t.Status.IsClosed && t.ResolvedDate < limit)
                .ToListAsync();

            foreach (var ticket in tickets)
            {
                _context.AuditLogs.Add(new AuditLogEnt
                {
                    TableName = "Tickets",
                    TableID = ticket.ID,
                    FieldName = "Durum",
                    OldValue = ticket.Status.Name,
                    NewValue = $"{closed.Name} (otomatik, {days} gün)"
                });
                ticket.StatusID = closed.ID;
                ticket.ClosedDate = now;
            }

            if (tickets.Count > 0)
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("{Count} talep otomatik kapatıldı.", tickets.Count);
            }
        }

        public async Task NotifyOverdueTicketsAsync()
        {
            long companyId = _context.CompanyId;
            DateTime now = DateTime.Now;

            var tickets = await _context.Tickets
                .Where(t => t.CompanyID == companyId && !t.IsCanceled && !t.OverdueNotified
                    && !t.Status.IsResolved && t.SlaPausedAt == null && t.DueDate < now)
                .ToListAsync();
            if (tickets.Count == 0) return;

            // Once isaretle ki mail hatasinda ayni talep her saat tekrar denenmesin.
            tickets.ForEach(t => t.OverdueNotified = true);
            await _context.SaveChangesAsync();

            foreach (var ticket in tickets)
                await _notifier.OverdueAsync(ticket.ID);
        }
    }
}
