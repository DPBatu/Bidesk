using Bidesk.Application.Abstraction.Services;
using Bidesk.Application.DTOs.User;
using Bidesk.Application.Exceptions;
using Bidesk.Application.Features;
using Bidesk.Application.Repositories.Public.Notification;
using Bidesk.Application.Repositories.Public.NotificationUser;
using Bidesk.Domain.Entities.Public;
using Bidesk.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Bidesk.Persistence.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationWriteRepository _wNotification;
        private readonly INotificationReadRepository _rNotification;
        private readonly INotificationUserReadRepository _rNotificationUser;
        private readonly INotificationUserWriteRepository _wNotificationUser;
        private readonly IContextService _sContext;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly NotificationHub _hubNotification;

        public NotificationService(
            INotificationWriteRepository wNotification,
            INotificationReadRepository rNotification,
            INotificationUserReadRepository rNotificationUser,
            INotificationUserWriteRepository wNotificationUser,
            IContextService sContext,
            IHubContext<NotificationHub> hubContext)
        {
            _wNotification = wNotification;
            _rNotification = rNotification;
            _rNotificationUser = rNotificationUser;
            _wNotificationUser = wNotificationUser;
            _sContext = sContext;
            _hubContext = hubContext;
            _hubNotification = new NotificationHub(_hubContext, _sContext);
        }

        public async Task AddAsync(NotificationDto notificationDto)
        {
            if (!notificationDto.Users.Any())
                throw new BaseException("Bildirim gönderilecek kullanıcıları belirtmediniz!", "Kullanıcı bilgisi eksik!");

            NotificationEnt entity = notificationDto.ToEntity();

            await _wNotification.AddAsync(entity);
            BaseRes response = await _wNotification.SaveAsync();

            if (!response.IsSuccess) throw new BaseException(response.ErrorMessage, "Hata oluştu!");

            NotificationDto result = new NotificationDto().FromEntity(entity);
            await _hubNotification.SendNotification(result);
        }

        public async Task<int> RemoveByUrlsAsync(params string[] urls)
        {
            string[] targets = (urls ?? Array.Empty<string>())
                .Where(u => !string.IsNullOrWhiteSpace(u))
                .Distinct()
                .ToArray();

            if (targets.Length == 0) return 0;

            // Bildirim listesi NotificationUser üzerinden okunuyor ve Notification'a IsCanceled
            // filtresi UYGULANMIYOR; bu yüzden asıl iptal edilmesi gereken kullanıcı satırlarıdır.
            List<NotificationUserEnt> notificationUsers = await _rNotificationUser
                .GetWhere(x => x.Notification != null && targets.Contains(x.Notification.Url))
                .ToListAsync();

            List<NotificationEnt> notifications = await _rNotification
                .GetWhere(x => targets.Contains(x.Url))
                .ToListAsync();

            if (notificationUsers.Count == 0 && notifications.Count == 0) return 0;

            if (notificationUsers.Count > 0) _wNotificationUser.RemoveRange(notificationUsers);
            if (notifications.Count > 0) _wNotification.RemoveRange(notifications);

            BaseRes response = await _wNotificationUser.SaveAsync();
            if (!response.IsSuccess) throw new BaseException(response.ErrorMessage, "Hata oluştu!");

            return notificationUsers.Count;
        }
    }
}
