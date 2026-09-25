using Bidesk.Application.Abstraction.Services.Ticket;
using Bidesk.Application.Abstraction.Services.User;
using Bidesk.Application.Extensions;
using Bidesk.Application.Features;
using Bidesk.Application.Features.Commands.Ticket.AddTicketConversation;
using Bidesk.Application.Features.Commands.Ticket.CreateTicket;
using Bidesk.Application.Features.Commands.Ticket.DeleteTicketAttachment;
using Bidesk.Application.Features.Commands.Ticket.PickUpTicket;
using Bidesk.Application.Features.Commands.Ticket.SaveTicketAttachment;
using Bidesk.Application.Features.Commands.Ticket.SaveTicketResolution;
using Bidesk.Application.Features.Commands.Ticket.UpdateTicket;
using Bidesk.Application.Features.Commands.Ticket.UpdateTicketField;
using Bidesk.Application.Features.Queries.Ticket.GetMyTicketsLazy;
using Bidesk.Application.Features.Queries.Ticket.GetTicketById;
using Bidesk.Application.Features.Queries.Ticket.GetTicketDashboard;
using Bidesk.Application.Features.Queries.Ticket.GetTicketHistory;
using Bidesk.Application.Features.Queries.Ticket.GetTicketsLazy;
using Bidesk.Application.ViewModels.Ticket;
using Bidesk.Domain.Entities;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context;
using Bidesk.Persistence.Handlers;
using Bidesk.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Bidesk.Persistence.Services.Ticket
{
    public class TicketService : ITicketService
    {
        private const string HistoryTable = "Tickets";

        private readonly BaseContext _context;
        private readonly IUserService _userService;
        private readonly TicketNotifier _notifier;

        public TicketService(BaseContext context, IUserService userService, TicketNotifier notifier)
        {
            _context = context;
            _userService = userService;
            _notifier = notifier;
        }

        #region Access

        /// <summary>Oturumdaki kullanicinin talep yetkileri.</summary>
        private sealed record Actor(long UserID, bool IsAdmin, List<long> GroupIDs)
        {
            /// <summary>En az bir grupta teknisyen ya da admin: talepleri yonetebilir.</summary>
            public bool IsTechnician => IsAdmin || GroupIDs.Count > 0;
        }

        private async Task<Actor> GetActorAsync(CancellationToken cancellationToken)
        {
            long userId = _userService.GetCurrentUserID();
            var user = await _userService.GetCurrentUserAsync();
            var groupIds = await Scoped<TechnicianGroupEnt>()
                .Where(t => t.UserID == userId)
                .Select(t => t.GroupID)
                .ToListAsync(cancellationToken);
            return new Actor(userId, user?.IsAdmin ?? false, groupIds);
        }

        private bool CanView(Actor actor, TicketEnt ticket)
            => actor.IsTechnician
               || ticket.RequesterUserID == actor.UserID
               || ticket.NotifyUsers.Any(n => !n.IsCanceled && n.UserID == actor.UserID);

        /// <summary>Sirket ve iptal suzgecli sorgu (ReadRepository.GetAll ile ayni kural).</summary>
        private IQueryable<T> Scoped<T>() where T : BaseEntity
        {
            long companyId = _context.CompanyId;
            return _context.Set<T>().Where(x => x.CompanyID == companyId && !x.IsCanceled);
        }

        #endregion

        #region Queries

        public async Task<GetMyTicketsLazyQRes> GetMyTicketsLazy(GetMyTicketsLazyQReq request, CancellationToken cancellationToken)
        {
            try
            {
                long userId = _userService.GetCurrentUserID();
                var query = Scoped<TicketEnt>().AsNoTracking()
                    .Where(t => t.RequesterUserID == userId || t.NotifyUsers.Any(n => !n.IsCanceled && n.UserID == userId));
                if (!request.IncludeClosed) query = query.Where(t => !t.Status.IsClosed);

                var result = await new GridQueryHandler<TicketGridVM>(query.Select(GridProjection),
                    (request.LazyLoad ?? string.Empty).ToGridQueryDto()).GetDataAsync(cancellationToken);

                return ResponseFactory.Success<GetMyTicketsLazyQRes>(configure: r => { r.Data = result.Data; r.TotalRecords = result.TotalRecords; });
            }
            catch (Exception ex) { return ResponseFactory.Error<GetMyTicketsLazyQRes>(ex); }
        }

        public async Task<GetTicketsLazyQRes> GetTicketsLazy(GetTicketsLazyQReq request, CancellationToken cancellationToken)
        {
            try
            {
                var actor = await GetActorAsync(cancellationToken);
                if (!actor.IsTechnician) return ResponseFactory.Fail<GetTicketsLazyQRes>("Bu liste yalnızca teknisyenlere açıktır.");

                DateTime now = DateTime.Now;
                var query = Scoped<TicketEnt>().AsNoTracking();
                query = request.ListType switch
                {
                    "mine" => query.Where(t => !t.Status.IsClosed && t.TechnicianUserID == actor.UserID),
                    "unassigned" => query.Where(t => !t.Status.IsClosed && t.TechnicianUserID == null),
                    "mygroups" => query.Where(t => !t.Status.IsClosed && actor.GroupIDs.Contains(t.GroupID)),
                    "overdue" => query.Where(t => !t.Status.IsResolved && t.SlaPausedAt == null && t.DueDate < now),
                    "resolved" => query.Where(t => t.Status.IsResolved && !t.Status.IsClosed),
                    "all" => query,
                    _ => query.Where(t => !t.Status.IsClosed)
                };

                if (request.StatusID is > 0) query = query.Where(t => t.StatusID == request.StatusID);
                if (request.PriorityID is > 0) query = query.Where(t => t.PriorityID == request.PriorityID);
                if (request.GroupID is > 0) query = query.Where(t => t.GroupID == request.GroupID);
                if (request.TechnicianUserID is > 0) query = query.Where(t => t.TechnicianUserID == request.TechnicianUserID);

                var result = await new GridQueryHandler<TicketGridVM>(query.Select(GridProjection),
                    (request.LazyLoad ?? string.Empty).ToGridQueryDto()).GetDataAsync(cancellationToken);

                return ResponseFactory.Success<GetTicketsLazyQRes>(configure: r => { r.Data = result.Data; r.TotalRecords = result.TotalRecords; });
            }
            catch (Exception ex) { return ResponseFactory.Error<GetTicketsLazyQRes>(ex); }
        }

        public async Task<GetTicketByIdQRes> GetTicketById(GetTicketByIdQReq request, CancellationToken cancellationToken)
        {
            try
            {
                var actor = await GetActorAsync(cancellationToken);
                var ticket = await Scoped<TicketEnt>().AsNoTracking()
                    .Include(t => t.RequesterUser).ThenInclude(u => u.Department)
                    .Include(t => t.TechnicianUser)
                    .Include(t => t.Status)
                    .Include(t => t.Priority)
                    .Include(t => t.Group)
                    .Include(t => t.Category)
                    .Include(t => t.SubCategory)
                    .Include(t => t.NotifyUsers.Where(n => !n.IsCanceled)).ThenInclude(n => n.User)
                    .Include(t => t.Conversations.Where(c => !c.IsCanceled)).ThenInclude(c => c.CreatedUser)
                    .Include(t => t.Attachments.Where(a => !a.IsCanceled))
                    .FirstOrDefaultAsync(t => t.ID == request.ID, cancellationToken);

                if (ticket == null) return ResponseFactory.Fail<GetTicketByIdQRes>("Talep bulunamadı.");
                if (!CanView(actor, ticket)) return ResponseFactory.Fail<GetTicketByIdQRes>("Bu talebi görüntüleme yetkiniz yok.");

                var attachments = ticket.Attachments.Select(a => new TicketAttachmentVM
                {
                    ID = a.ID, TicketID = a.TicketID, ConversationID = a.ConversationID, AttachmentID = a.AttachmentID, Name = a.Name
                }).ToList();

                var detail = new TicketDetailVM
                {
                    ID = ticket.ID,
                    Subject = ticket.Subject,
                    Description = ticket.Description,
                    RequesterUserID = ticket.RequesterUserID,
                    RequesterName = ticket.RequesterUser.NameSurname,
                    RequesterEmail = ticket.RequesterUser.Email,
                    RequesterPhone = ticket.RequesterUser.CompanyPhoneNumber ?? ticket.RequesterUser.IntercomNumber,
                    RequesterDepartment = ticket.RequesterUser.Department?.Description,
                    TechnicianUserID = ticket.TechnicianUserID,
                    TechnicianName = ticket.TechnicianUser?.NameSurname,
                    StatusID = ticket.StatusID,
                    StatusName = ticket.Status.Name,
                    StatusSeverity = ticket.Status.Severity,
                    IsClosed = ticket.Status.IsClosed,
                    PriorityID = ticket.PriorityID,
                    PriorityName = ticket.Priority.Name,
                    PrioritySeverity = ticket.Priority.Severity,
                    GroupID = ticket.GroupID,
                    GroupName = ticket.Group.Name,
                    CategoryID = ticket.CategoryID,
                    CategoryName = ticket.Category?.Name,
                    SubCategoryID = ticket.SubCategoryID,
                    SubCategoryName = ticket.SubCategory?.Name,
                    CreatedDate = ticket.CreatedDate,
                    UpdatedDate = ticket.UpdatedDate,
                    DueDate = ticket.DueDate,
                    IsOverdue = !ticket.Status.IsResolved && ticket.SlaPausedAt == null && ticket.DueDate < DateTime.Now,
                    FirstResponseDate = ticket.FirstResponseDate,
                    ResolvedDate = ticket.ResolvedDate,
                    ClosedDate = ticket.ClosedDate,
                    Resolution = ticket.Resolution,
                    HasAttachment = attachments.Count > 0,
                    NotifyUserIDs = ticket.NotifyUsers.Select(n => n.UserID).ToList(),
                    NotifyUsers = ticket.NotifyUsers.Select(n => new TechnicianVM { UserID = n.UserID, NameSurname = n.User.NameSurname }).ToList(),
                    Attachments = attachments.Where(a => a.ConversationID == null).ToList(),
                    Conversations = ticket.Conversations
                        .Where(c => actor.IsTechnician || !c.IsInternal)
                        .OrderBy(c => c.CreatedDate)
                        .Select(c => new TicketConversationVM
                        {
                            ID = c.ID,
                            Body = c.Body,
                            IsInternal = c.IsInternal,
                            CreatedUserId = c.CreatedUserId,
                            CreatedUserName = c.CreatedUser.NameSurname,
                            CreatedDate = c.CreatedDate,
                            Attachments = attachments.Where(a => a.ConversationID == c.ID).ToList()
                        }).ToList(),
                    CanManage = actor.IsTechnician
                };

                return ResponseFactory.Success<GetTicketByIdQRes>(configure: r => r.Ticket = detail);
            }
            catch (Exception ex) { return ResponseFactory.Error<GetTicketByIdQRes>(ex); }
        }

        public async Task<GetTicketHistoryQRes> GetTicketHistory(GetTicketHistoryQReq request, CancellationToken cancellationToken)
        {
            try
            {
                var actor = await GetActorAsync(cancellationToken);
                var ticket = await Scoped<TicketEnt>().AsNoTracking().Include(t => t.NotifyUsers)
                    .FirstOrDefaultAsync(t => t.ID == request.ID, cancellationToken);
                if (ticket == null) return ResponseFactory.Fail<GetTicketHistoryQRes>("Talep bulunamadı.");
                if (!CanView(actor, ticket)) return ResponseFactory.Fail<GetTicketHistoryQRes>("Bu talebi görüntüleme yetkiniz yok.");

                var history = await Scoped<AuditLogEnt>().AsNoTracking()
                    .Where(h => h.TableName == HistoryTable && h.TableID == request.ID)
                    .OrderByDescending(h => h.CreatedDate).ThenByDescending(h => h.ID)
                    .Select(h => new TicketHistoryVM
                    {
                        FieldName = h.FieldName,
                        OldValue = h.OldValue,
                        NewValue = h.NewValue,
                        CreatedDate = h.CreatedDate,
                        CreatedUserName = h.CreatedUser.NameSurname
                    })
                    .ToListAsync(cancellationToken);

                return ResponseFactory.Success<GetTicketHistoryQRes>(configure: r => r.History = history);
            }
            catch (Exception ex) { return ResponseFactory.Error<GetTicketHistoryQRes>(ex); }
        }

        public async Task<GetTicketDashboardQRes> GetTicketDashboard(GetTicketDashboardQReq request, CancellationToken cancellationToken)
        {
            try
            {
                var actor = await GetActorAsync(cancellationToken);
                if (!actor.IsTechnician) return ResponseFactory.Fail<GetTicketDashboardQRes>("Bu ekran yalnızca teknisyenlere açıktır.");

                DateTime now = DateTime.Now;
                DateTime since = now.AddDays(-30);
                var open = Scoped<TicketEnt>().AsNoTracking().Where(t => !t.Status.IsClosed);

                var dashboard = new TicketDashboardVM
                {
                    OpenCount = await open.CountAsync(cancellationToken),
                    UnassignedCount = await open.CountAsync(t => t.TechnicianUserID == null, cancellationToken),
                    OverdueCount = await open.CountAsync(t => !t.Status.IsResolved && t.SlaPausedAt == null && t.DueDate < now, cancellationToken),
                    MyOpenCount = await open.CountAsync(t => t.TechnicianUserID == actor.UserID, cancellationToken),
                    ResolvedLast30Days = await Scoped<TicketEnt>().CountAsync(t => t.ResolvedDate >= since, cancellationToken),
                    ByStatus = await open.GroupBy(t => new { t.StatusID, t.Status.Name, t.Status.Severity, t.Status.SortOrder })
                        .OrderBy(g => g.Key.SortOrder)
                        .Select(g => new TicketCountVM { ID = g.Key.StatusID, Name = g.Key.Name, Severity = g.Key.Severity, Count = g.Count() })
                        .ToListAsync(cancellationToken),
                    ByGroup = await open.GroupBy(t => new { t.GroupID, t.Group.Name })
                        .Select(g => new TicketCountVM { ID = g.Key.GroupID, Name = g.Key.Name, Count = g.Count() })
                        .OrderByDescending(g => g.Count)
                        .ToListAsync(cancellationToken),
                    ByTechnician = await open.Where(t => t.TechnicianUserID != null)
                        .GroupBy(t => new { t.TechnicianUserID, t.TechnicianUser!.NameSurname })
                        .Select(g => new TicketCountVM { ID = g.Key.TechnicianUserID!.Value, Name = g.Key.NameSurname, Count = g.Count() })
                        .OrderByDescending(g => g.Count)
                        .ToListAsync(cancellationToken)
                };

                return ResponseFactory.Success<GetTicketDashboardQRes>(configure: r => r.Dashboard = dashboard);
            }
            catch (Exception ex) { return ResponseFactory.Error<GetTicketDashboardQRes>(ex); }
        }

        #endregion

        #region Create / Update

        public async Task<CreateTicketCmdRes> CreateTicket(CreateTicketCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Subject))
                    return ResponseFactory.Fail<CreateTicketCmdRes>("Konu zorunludur.");
                if (IsBlankHtml(request.Description))
                    return ResponseFactory.Fail<CreateTicketCmdRes>("Açıklama zorunludur.");

                var group = await Scoped<TicketGroupEnt>().Include(g => g.Technicians.Where(t => !t.IsCanceled))
                    .FirstOrDefaultAsync(g => g.ID == request.GroupID && g.IsActive, cancellationToken);
                if (group == null) return ResponseFactory.Fail<CreateTicketCmdRes>("Grup seçimi zorunludur.");

                // Oncelik bos gelirse varsayilan (Belirtilmemis).
                var priority = request.PriorityID is > 0
                    ? await Scoped<TicketPriorityEnt>().FirstOrDefaultAsync(p => p.ID == request.PriorityID && p.IsActive, cancellationToken)
                    : await Scoped<TicketPriorityEnt>().FirstOrDefaultAsync(p => p.IsDefault, cancellationToken);
                if (priority == null) return ResponseFactory.Fail<CreateTicketCmdRes>("Öncelik bulunamadı.");

                string? categoryError = await ValidateCategoryAsync(request.CategoryID, request.SubCategoryID, cancellationToken);
                if (categoryError != null) return ResponseFactory.Fail<CreateTicketCmdRes>(categoryError);

                // Durum istemciden alinmaz: her zaman varsayilan (Talep).
                var status = await Scoped<TicketStatusEnt>().FirstOrDefaultAsync(s => s.IsDefault, cancellationToken);
                if (status == null) return ResponseFactory.Fail<CreateTicketCmdRes>("Varsayılan durum tanımlı değil.");

                DateTime now = DateTime.Now;
                var ticket = new TicketEnt
                {
                    Subject = request.Subject.Trim(),
                    Description = request.Description,
                    RequesterUserID = _userService.GetCurrentUserID(),
                    StatusID = status.ID,
                    PriorityID = priority.ID,
                    GroupID = group.ID,
                    // Grupta tek teknisyen varsa dogrudan ona; birden fazlaysa atanmamis kalir.
                    TechnicianUserID = group.Technicians.Count == 1 ? group.Technicians.First().UserID : null,
                    CategoryID = request.CategoryID is > 0 ? request.CategoryID : null,
                    SubCategoryID = request.SubCategoryID is > 0 ? request.SubCategoryID : null,
                    DueDate = priority.ResolveHours.HasValue ? now.AddHours(priority.ResolveHours.Value) : null
                };

                foreach (long userId in request.NotifyUserIDs.Distinct().Where(id => id != ticket.RequesterUserID))
                    ticket.NotifyUsers.Add(new TicketNotifyUserEnt { UserID = userId });

                foreach (var a in request.Attachments.Where(a => a.AttachmentID is > 0))
                    ticket.Attachments.Add(new TicketAttachmentEnt { AttachmentID = a.AttachmentID!.Value, Name = a.Name });

                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync(cancellationToken);

                await AddHistoryAsync(ticket.ID, now, ("Talep", null, "Oluşturuldu"));
                await _context.SaveChangesAsync(cancellationToken);

                await _notifier.TicketCreatedAsync(ticket);
                return ResponseFactory.Success<CreateTicketCmdRes>($"Talebiniz #{ticket.ID} numarasıyla oluşturuldu.", r => r.Id = ticket.ID);
            }
            catch (Exception ex) { return ResponseFactory.Error<CreateTicketCmdRes>(ex); }
        }

        public async Task<UpdateTicketCmdRes> UpdateTicket(UpdateTicketCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                var actor = await GetActorAsync(cancellationToken);
                if (!actor.IsTechnician) return ResponseFactory.Fail<UpdateTicketCmdRes>("Talebi düzenleme yetkiniz yok.");
                if (string.IsNullOrWhiteSpace(request.Subject)) return ResponseFactory.Fail<UpdateTicketCmdRes>("Konu zorunludur.");
                if (IsBlankHtml(request.Description)) return ResponseFactory.Fail<UpdateTicketCmdRes>("Açıklama zorunludur.");

                var ticket = await LoadForUpdateAsync(request.ID, cancellationToken);
                if (ticket == null) return ResponseFactory.Fail<UpdateTicketCmdRes>("Talep bulunamadı.");

                string? categoryError = await ValidateCategoryAsync(request.CategoryID, request.SubCategoryID, cancellationToken);
                if (categoryError != null) return ResponseFactory.Fail<UpdateTicketCmdRes>(categoryError);

                var changes = new List<(string, string?, string?)>();
                var change = new TicketChange
                {
                    StatusID = request.StatusID,
                    PriorityID = request.PriorityID,
                    GroupID = request.GroupID,
                    TechnicianUserID = request.TechnicianUserID,
                    SetTechnician = true
                };
                string? error = await ApplyChangeAsync(ticket, change, changes, cancellationToken);
                if (error != null) return ResponseFactory.Fail<UpdateTicketCmdRes>(error);

                if (ticket.Subject != request.Subject.Trim())
                {
                    changes.Add(("Konu", ticket.Subject, request.Subject.Trim()));
                    ticket.Subject = request.Subject.Trim();
                }
                if (ticket.Description != request.Description)
                {
                    changes.Add(("Açıklama", null, "Güncellendi"));
                    ticket.Description = request.Description;
                }

                long? categoryId = request.CategoryID is > 0 ? request.CategoryID : null;
                long? subCategoryId = request.SubCategoryID is > 0 ? request.SubCategoryID : null;
                if (ticket.CategoryID != categoryId || ticket.SubCategoryID != subCategoryId)
                {
                    string? oldName = await NameOfAsync<TicketCategoryEnt>(ticket.CategoryID, c => c.Name, cancellationToken);
                    string? oldSub = await NameOfAsync<TicketSubCategoryEnt>(ticket.SubCategoryID, c => c.Name, cancellationToken);
                    string? newName = await NameOfAsync<TicketCategoryEnt>(categoryId, c => c.Name, cancellationToken);
                    string? newSub = await NameOfAsync<TicketSubCategoryEnt>(subCategoryId, c => c.Name, cancellationToken);
                    changes.Add(("Kategori", JoinCategory(oldName, oldSub), JoinCategory(newName, newSub)));
                    ticket.CategoryID = categoryId;
                    ticket.SubCategoryID = subCategoryId;
                }

                // Bildirim listesini esitle.
                var wanted = request.NotifyUserIDs.Distinct().Where(id => id != ticket.RequesterUserID).ToList();
                var current = ticket.NotifyUsers.Where(n => !n.IsCanceled).ToList();
                var removed = current.Where(n => !wanted.Contains(n.UserID)).ToList();
                var added = wanted.Where(id => !current.Any(n => n.UserID == id)).ToList();
                removed.ForEach(n => n.IsCanceled = true);
                added.ForEach(id => ticket.NotifyUsers.Add(new TicketNotifyUserEnt { UserID = id }));
                if (removed.Count > 0 || added.Count > 0) changes.Add(("Bildirim Listesi", null, "Güncellendi"));

                return await SaveChangesAsync<UpdateTicketCmdRes>(ticket, changes, change, "Talep güncellendi.", cancellationToken);
            }
            catch (Exception ex) { return ResponseFactory.Error<UpdateTicketCmdRes>(ex); }
        }

        public async Task<UpdateTicketFieldCmdRes> UpdateTicketField(UpdateTicketFieldCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                var actor = await GetActorAsync(cancellationToken);
                if (!actor.IsTechnician) return ResponseFactory.Fail<UpdateTicketFieldCmdRes>("Talebi düzenleme yetkiniz yok.");

                var ticket = await LoadForUpdateAsync(request.ID, cancellationToken);
                if (ticket == null) return ResponseFactory.Fail<UpdateTicketFieldCmdRes>("Talep bulunamadı.");

                var change = new TicketChange();
                switch (request.Field?.ToLowerInvariant())
                {
                    case "status":
                        if (request.Value is not > 0) return ResponseFactory.Fail<UpdateTicketFieldCmdRes>("Durum seçiniz.");
                        change.StatusID = request.Value.Value;
                        break;
                    case "priority":
                        if (request.Value is not > 0) return ResponseFactory.Fail<UpdateTicketFieldCmdRes>("Öncelik seçiniz.");
                        change.PriorityID = request.Value.Value;
                        break;
                    case "group":
                        if (request.Value is not > 0) return ResponseFactory.Fail<UpdateTicketFieldCmdRes>("Grup seçiniz.");
                        change.GroupID = request.Value.Value;
                        break;
                    case "technician":
                        change.TechnicianUserID = request.Value is > 0 ? request.Value : null;
                        change.SetTechnician = true;
                        break;
                    default:
                        return ResponseFactory.Fail<UpdateTicketFieldCmdRes>("Geçersiz alan.");
                }

                var changes = new List<(string, string?, string?)>();
                string? error = await ApplyChangeAsync(ticket, change, changes, cancellationToken);
                if (error != null) return ResponseFactory.Fail<UpdateTicketFieldCmdRes>(error);

                return await SaveChangesAsync<UpdateTicketFieldCmdRes>(ticket, changes, change, "Talep güncellendi.", cancellationToken);
            }
            catch (Exception ex) { return ResponseFactory.Error<UpdateTicketFieldCmdRes>(ex); }
        }

        public async Task<PickUpTicketCmdRes> PickUpTicket(PickUpTicketCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                var actor = await GetActorAsync(cancellationToken);
                var ticket = await LoadForUpdateAsync(request.ID, cancellationToken);
                if (ticket == null) return ResponseFactory.Fail<PickUpTicketCmdRes>("Talep bulunamadı.");
                if (!actor.IsAdmin && !actor.GroupIDs.Contains(ticket.GroupID))
                    return ResponseFactory.Fail<PickUpTicketCmdRes>("Yalnızca talebin grubundaki teknisyenler üzerine alabilir.");
                if (ticket.TechnicianUserID == actor.UserID)
                    return ResponseFactory.Fail<PickUpTicketCmdRes>("Talep zaten sizde.");

                var change = new TicketChange { TechnicianUserID = actor.UserID, SetTechnician = true };
                var changes = new List<(string, string?, string?)>();
                string? error = await ApplyChangeAsync(ticket, change, changes, cancellationToken, allowAnyTechnician: actor.IsAdmin);
                if (error != null) return ResponseFactory.Fail<PickUpTicketCmdRes>(error);

                ticket.FirstResponseDate ??= DateTime.Now;
                return await SaveChangesAsync<PickUpTicketCmdRes>(ticket, changes, change, "Talep üzerinize alındı.", cancellationToken);
            }
            catch (Exception ex) { return ResponseFactory.Error<PickUpTicketCmdRes>(ex); }
        }

        public async Task<AddTicketConversationCmdRes> AddTicketConversation(AddTicketConversationCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                if (IsBlankHtml(request.Body)) return ResponseFactory.Fail<AddTicketConversationCmdRes>("Mesaj boş olamaz.");

                var actor = await GetActorAsync(cancellationToken);
                var ticket = await LoadForUpdateAsync(request.TicketID, cancellationToken);
                if (ticket == null) return ResponseFactory.Fail<AddTicketConversationCmdRes>("Talep bulunamadı.");
                if (!CanView(actor, ticket)) return ResponseFactory.Fail<AddTicketConversationCmdRes>("Bu talebe yazma yetkiniz yok.");
                if (ticket.Status.IsClosed) return ResponseFactory.Fail<AddTicketConversationCmdRes>("Kapanmış talebe yanıt yazılamaz.");
                if (request.IsInternal && !actor.IsTechnician) return ResponseFactory.Fail<AddTicketConversationCmdRes>("Not ekleme yetkiniz yok.");

                bool fromHandler = actor.IsTechnician && ticket.RequesterUserID != actor.UserID;
                var message = new TicketConversationEnt { TicketID = ticket.ID, Body = request.Body, IsInternal = request.IsInternal };
                ticket.Conversations.Add(message);

                var change = new TicketChange();
                var changes = new List<(string, string?, string?)>();
                if (fromHandler)
                {
                    if (!request.IsInternal) ticket.FirstResponseDate ??= DateTime.Now;
                    if (request.StatusID is > 0) change.StatusID = request.StatusID.Value;
                }
                else if (ticket.Status.IsResolved || ticket.Status.PausesSla)
                {
                    // Istek sahibi yanit verdi: bilgi bekleniyorsa ya da cozuldu itiraziysa talep yeniden acilir.
                    var defaultStatus = await Scoped<TicketStatusEnt>().FirstAsync(s => s.IsDefault, cancellationToken);
                    change.StatusID = defaultStatus.ID;
                }

                string? error = await ApplyChangeAsync(ticket, change, changes, cancellationToken);
                if (error != null) return ResponseFactory.Fail<AddTicketConversationCmdRes>(error);

                await _context.SaveChangesAsync(cancellationToken);

                foreach (var a in request.Attachments.Where(a => a.AttachmentID is > 0))
                    ticket.Attachments.Add(new TicketAttachmentEnt { AttachmentID = a.AttachmentID!.Value, Name = a.Name, ConversationID = message.ID });

                var res = await SaveChangesAsync<AddTicketConversationCmdRes>(ticket, changes, change,
                    request.IsInternal ? "Not eklendi." : "Yanıt gönderildi.", cancellationToken);
                if (res.IsSuccess) await _notifier.ConversationAddedAsync(ticket.ID, message, fromHandler);
                return res;
            }
            catch (Exception ex) { return ResponseFactory.Error<AddTicketConversationCmdRes>(ex); }
        }

        public async Task<SaveTicketResolutionCmdRes> SaveTicketResolution(SaveTicketResolutionCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                var actor = await GetActorAsync(cancellationToken);
                if (!actor.IsTechnician) return ResponseFactory.Fail<SaveTicketResolutionCmdRes>("Çözümleme yazma yetkiniz yok.");

                var ticket = await LoadForUpdateAsync(request.ID, cancellationToken);
                if (ticket == null) return ResponseFactory.Fail<SaveTicketResolutionCmdRes>("Talep bulunamadı.");
                if (request.MarkResolved && IsBlankHtml(request.Resolution))
                    return ResponseFactory.Fail<SaveTicketResolutionCmdRes>("Çözüldü olarak işaretlemek için çözümleme yazınız.");

                var changes = new List<(string, string?, string?)>();
                if (ticket.Resolution != request.Resolution)
                {
                    changes.Add(("Çözümleme", null, "Güncellendi"));
                    ticket.Resolution = request.Resolution;
                }

                var change = new TicketChange();
                if (request.MarkResolved && !ticket.Status.IsResolved)
                {
                    var resolved = await Scoped<TicketStatusEnt>()
                        .Where(s => s.IsActive && s.IsResolved && !s.IsClosed)
                        .OrderBy(s => s.SortOrder)
                        .FirstOrDefaultAsync(cancellationToken);
                    if (resolved == null) return ResponseFactory.Fail<SaveTicketResolutionCmdRes>("\"Çözüldü\" tipinde bir durum tanımlı değil.");
                    change.StatusID = resolved.ID;
                }

                string? error = await ApplyChangeAsync(ticket, change, changes, cancellationToken);
                if (error != null) return ResponseFactory.Fail<SaveTicketResolutionCmdRes>(error);

                return await SaveChangesAsync<SaveTicketResolutionCmdRes>(ticket, changes, change, "Çözümleme kaydedildi.", cancellationToken);
            }
            catch (Exception ex) { return ResponseFactory.Error<SaveTicketResolutionCmdRes>(ex); }
        }

        public async Task<SaveTicketAttachmentCmdRes> SaveTicketAttachment(SaveTicketAttachmentCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                var actor = await GetActorAsync(cancellationToken);
                var ticket = await LoadForUpdateAsync(request.TicketID, cancellationToken);
                if (ticket == null) return ResponseFactory.Fail<SaveTicketAttachmentCmdRes>("Talep bulunamadı.");
                if (!CanView(actor, ticket)) return ResponseFactory.Fail<SaveTicketAttachmentCmdRes>("Bu talebe dosya ekleme yetkiniz yok.");

                var existing = ticket.Attachments.Where(a => !a.IsCanceled).Select(a => a.AttachmentID).ToHashSet();
                int count = 0;
                foreach (var a in request.Attachments.Where(a => a.AttachmentID is > 0 && !existing.Contains(a.AttachmentID!.Value)))
                {
                    ticket.Attachments.Add(new TicketAttachmentEnt { AttachmentID = a.AttachmentID!.Value, Name = a.Name });
                    existing.Add(a.AttachmentID!.Value);
                    count++;
                }
                if (count == 0) return ResponseFactory.Success<SaveTicketAttachmentCmdRes>("Eklenecek dosya bulunmuyor.");

                var changes = new List<(string, string?, string?)> { ("Ek", null, $"{count} dosya eklendi") };
                return await SaveChangesAsync<SaveTicketAttachmentCmdRes>(ticket, changes, new TicketChange(), "Dosyalar eklendi.", cancellationToken);
            }
            catch (Exception ex) { return ResponseFactory.Error<SaveTicketAttachmentCmdRes>(ex); }
        }

        public async Task<DeleteTicketAttachmentCmdRes> DeleteTicketAttachment(DeleteTicketAttachmentCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                var actor = await GetActorAsync(cancellationToken);
                var attachment = await Scoped<TicketAttachmentEnt>().FirstOrDefaultAsync(a => a.ID == request.ID, cancellationToken);
                if (attachment == null) return ResponseFactory.Fail<DeleteTicketAttachmentCmdRes>("Dosya bulunamadı.");
                if (!actor.IsTechnician && attachment.CreatedUserId != actor.UserID)
                    return ResponseFactory.Fail<DeleteTicketAttachmentCmdRes>("Yalnızca kendi eklediğiniz dosyayı silebilirsiniz.");

                attachment.IsCanceled = true;
                await AddHistoryAsync(attachment.TicketID, DateTime.Now, ("Ek", attachment.Name, "Silindi"));
                await _context.SaveChangesAsync(cancellationToken);
                return ResponseFactory.Success<DeleteTicketAttachmentCmdRes>("Dosya silindi.");
            }
            catch (Exception ex) { return ResponseFactory.Error<DeleteTicketAttachmentCmdRes>(ex); }
        }

        #endregion

        #region Change engine

        /// <summary>Durum / oncelik / grup / teknisyen degisikligi; null alan degismez.</summary>
        private sealed class TicketChange
        {
            public long? StatusID { get; set; }
            public long? PriorityID { get; set; }
            public long? GroupID { get; set; }
            public long? TechnicianUserID { get; set; }
            public bool SetTechnician { get; set; }

            public bool StatusChanged { get; set; }
            public bool TechnicianAssigned { get; set; }
        }

        /// <summary>
        /// Degisikligi dogrular, uygular ve SLA alanlarini gunceller. Hata varsa mesaj doner ve
        /// talebe dokunmaz. Degisen her alan okunabilir adlarla <paramref name="changes"/>'e eklenir.
        /// </summary>
        private async Task<string?> ApplyChangeAsync(TicketEnt ticket, TicketChange change,
            List<(string, string?, string?)> changes, CancellationToken cancellationToken, bool allowAnyTechnician = false)
        {
            DateTime now = DateTime.Now;

            TicketStatusEnt? newStatus = null;
            if (change.StatusID.HasValue && change.StatusID != ticket.StatusID)
            {
                newStatus = await Scoped<TicketStatusEnt>().FirstOrDefaultAsync(s => s.ID == change.StatusID && s.IsActive, cancellationToken);
                if (newStatus == null) return "Durum bulunamadı.";
            }

            TicketPriorityEnt? newPriority = null;
            if (change.PriorityID.HasValue && change.PriorityID != ticket.PriorityID)
            {
                newPriority = await Scoped<TicketPriorityEnt>().FirstOrDefaultAsync(p => p.ID == change.PriorityID && p.IsActive, cancellationToken);
                if (newPriority == null) return "Öncelik bulunamadı.";
            }

            long targetGroupId = change.GroupID ?? ticket.GroupID;
            TicketGroupEnt? newGroup = null;
            if (targetGroupId != ticket.GroupID)
            {
                newGroup = await Scoped<TicketGroupEnt>().FirstOrDefaultAsync(g => g.ID == targetGroupId && g.IsActive, cancellationToken);
                if (newGroup == null) return "Grup bulunamadı.";
            }

            var groupTechnicians = await Scoped<TechnicianGroupEnt>()
                .Where(t => t.GroupID == targetGroupId)
                .Select(t => t.UserID)
                .ToListAsync(cancellationToken);

            long? targetTechnician = change.SetTechnician ? change.TechnicianUserID : ticket.TechnicianUserID;
            if (targetTechnician.HasValue && !groupTechnicians.Contains(targetTechnician.Value))
            {
                if (change.SetTechnician && !allowAnyTechnician) return "Seçilen teknisyen bu grupta tanımlı değil.";
                // Grup degisti ve eski teknisyen yeni grupta yok: atama duser.
                if (!change.SetTechnician) targetTechnician = null;
            }
            // Grup degisti, teknisyen yok ve yeni grupta tek teknisyen var: ona ata.
            if (newGroup != null && !targetTechnician.HasValue && groupTechnicians.Count == 1)
                targetTechnician = groupTechnicians[0];

            // --- uygula ---
            if (newGroup != null)
            {
                changes.Add(("Grup", await NameOfAsync<TicketGroupEnt>(ticket.GroupID, g => g.Name, cancellationToken), newGroup.Name));
                ticket.GroupID = newGroup.ID;
            }

            if (targetTechnician != ticket.TechnicianUserID)
            {
                string? oldName = await UserNameAsync(ticket.TechnicianUserID, cancellationToken);
                string? newName = await UserNameAsync(targetTechnician, cancellationToken);
                changes.Add(("Teknisyen", oldName ?? "Atanmamış", newName ?? "Atanmamış"));
                ticket.TechnicianUserID = targetTechnician;
                change.TechnicianAssigned = targetTechnician.HasValue;
            }

            if (newPriority != null)
            {
                changes.Add(("Öncelik", ticket.Priority.Name, newPriority.Name));
                ticket.DueDate = RecalculateDueDate(ticket, ticket.Priority, newPriority);
                ticket.PriorityID = newPriority.ID;
                ticket.Priority = newPriority;
                ticket.OverdueNotified = false;
            }

            if (newStatus != null)
            {
                var oldStatus = ticket.Status;
                changes.Add(("Durum", oldStatus.Name, newStatus.Name));

                // SLA: duraklatan duruma giriste sayaci durdur, cikista hedefi beklenen sure kadar ilerlet.
                if (newStatus.PausesSla && ticket.SlaPausedAt == null)
                    ticket.SlaPausedAt = now;
                else if (!newStatus.PausesSla && ticket.SlaPausedAt.HasValue)
                {
                    if (ticket.DueDate.HasValue) ticket.DueDate = ticket.DueDate.Value + (now - ticket.SlaPausedAt.Value);
                    ticket.OverdueNotified = false;
                    ticket.SlaPausedAt = null;
                }

                if (newStatus.IsResolved && !oldStatus.IsResolved) ticket.ResolvedDate = now;
                if (!newStatus.IsResolved) ticket.ResolvedDate = null;
                ticket.ClosedDate = newStatus.IsClosed ? (ticket.ClosedDate ?? now) : null;

                ticket.StatusID = newStatus.ID;
                ticket.Status = newStatus;
                change.StatusChanged = true;
            }

            return null;
        }

        /// <summary>Oncelik degisince hedef tarih yeni sureye gore; daha once biriken bekleme kaymasi korunur.</summary>
        private static DateTime? RecalculateDueDate(TicketEnt ticket, TicketPriorityEnt oldPriority, TicketPriorityEnt newPriority)
        {
            if (!newPriority.ResolveHours.HasValue) return null;

            TimeSpan pauseShift = TimeSpan.Zero;
            if (ticket.DueDate.HasValue && oldPriority.ResolveHours.HasValue)
                pauseShift = ticket.DueDate.Value - ticket.CreatedDate.AddHours(oldPriority.ResolveHours.Value);
            if (pauseShift < TimeSpan.Zero) pauseShift = TimeSpan.Zero;

            return ticket.CreatedDate.AddHours(newPriority.ResolveHours.Value) + pauseShift;
        }

        private async Task<T> SaveChangesAsync<T>(TicketEnt ticket, List<(string, string?, string?)> changes,
            TicketChange change, string message, CancellationToken cancellationToken) where T : BaseRes, new()
        {
            if (changes.Count > 0) await AddHistoryAsync(ticket.ID, DateTime.Now, changes.ToArray());
            await _context.SaveChangesAsync(cancellationToken);

            if (change.TechnicianAssigned && ticket.TechnicianUserID != _userService.GetCurrentUserID())
                await _notifier.TechnicianAssignedAsync(ticket.ID);
            if (change.StatusChanged) await _notifier.StatusChangedAsync(ticket.ID);

            return ResponseFactory.Success<T>(message, r => r.Id = ticket.ID);
        }

        #endregion

        #region Helpers

        private static readonly Expression<Func<TicketEnt, TicketGridVM>> GridProjection = t => new TicketGridVM
        {
            ID = t.ID,
            Subject = t.Subject,
            RequesterUserID = t.RequesterUserID,
            RequesterName = t.RequesterUser.NameSurname,
            TechnicianUserID = t.TechnicianUserID,
            TechnicianName = t.TechnicianUser != null ? t.TechnicianUser.NameSurname : null,
            StatusID = t.StatusID,
            StatusName = t.Status.Name,
            StatusSeverity = t.Status.Severity,
            IsClosed = t.Status.IsClosed,
            PriorityID = t.PriorityID,
            PriorityName = t.Priority.Name,
            PrioritySeverity = t.Priority.Severity,
            GroupID = t.GroupID,
            GroupName = t.Group.Name,
            CategoryName = t.Category != null ? t.Category.Name : null,
            SubCategoryName = t.SubCategory != null ? t.SubCategory.Name : null,
            CreatedDate = t.CreatedDate,
            DueDate = t.DueDate,
            IsOverdue = !t.Status.IsResolved && t.SlaPausedAt == null && t.DueDate < DateTime.Now,
            HasAttachment = t.Attachments.Any(a => !a.IsCanceled)
        };

        private Task<TicketEnt?> LoadForUpdateAsync(long id, CancellationToken cancellationToken)
            => Scoped<TicketEnt>()
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include(t => t.NotifyUsers)
                .Include(t => t.Attachments)
                .FirstOrDefaultAsync(t => t.ID == id, cancellationToken);

        private async Task<string?> ValidateCategoryAsync(long? categoryId, long? subCategoryId, CancellationToken cancellationToken)
        {
            if (subCategoryId is > 0 && categoryId is not > 0) return "Alt kategori için kategori seçiniz.";
            if (categoryId is > 0 && !await Scoped<TicketCategoryEnt>().AnyAsync(c => c.ID == categoryId, cancellationToken))
                return "Kategori bulunamadı.";
            if (subCategoryId is > 0 && !await Scoped<TicketSubCategoryEnt>().AnyAsync(s => s.ID == subCategoryId && s.CategoryID == categoryId, cancellationToken))
                return "Alt kategori seçilen kategoriye ait değil.";
            return null;
        }

        private async Task AddHistoryAsync(long ticketId, DateTime date, params (string Field, string? Old, string? New)[] entries)
        {
            foreach (var e in entries)
            {
                await _context.AuditLogs.AddAsync(new AuditLogEnt
                {
                    TableName = HistoryTable,
                    TableID = ticketId,
                    FieldName = e.Field,
                    OldValue = e.Old,
                    NewValue = e.New
                });
            }
        }

        private async Task<string?> NameOfAsync<T>(long? id, Expression<Func<T, string>> name, CancellationToken cancellationToken) where T : BaseEntity
            => id.HasValue ? await Scoped<T>().Where(x => x.ID == id).Select(name).FirstOrDefaultAsync(cancellationToken) : null;

        private async Task<string?> UserNameAsync(long? userId, CancellationToken cancellationToken)
            => userId.HasValue
                ? await _context.AspNetUsers.Where(u => u.Id == userId).Select(u => u.NameSurname).FirstOrDefaultAsync(cancellationToken)
                : null;

        private static string? JoinCategory(string? category, string? sub)
            => category == null ? null : sub == null ? category : $"{category} / {sub}";

        /// <summary>Zengin metin editoru bos paragraf ("&lt;p&gt;&lt;/p&gt;") gonderebilir; etiketsiz metne bakilir.</summary>
        private static bool IsBlankHtml(string? html)
            => string.IsNullOrWhiteSpace(html)
               || string.IsNullOrWhiteSpace(Regex.Replace(html, "<[^>]*>|&nbsp;", string.Empty))
                  && !html.Contains("<img", StringComparison.OrdinalIgnoreCase);

        #endregion
    }
}
