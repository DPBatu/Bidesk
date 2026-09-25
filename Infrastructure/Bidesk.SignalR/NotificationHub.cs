using Bidesk.Application.Abstraction.Services;
using Bidesk.Application.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Bidesk.SignalR
{
    [Authorize(AuthenticationSchemes = "Loggedin")]
    public class NotificationHub : Hub
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IContextService _contextService;

        public NotificationHub(IHubContext<NotificationHub> hubContext, IContextService contextService)
        {
            _hubContext = hubContext;
            _contextService = contextService;
        }

        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, _contextService.GetCurrentUserId().ToString());
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, _contextService.GetCurrentUserId().ToString());
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(NotificationDto notification)
        {
            var notifications = notification.ToFlatList();

            for (int i = 0; i < notifications.Count; i++)
            {
                await _hubContext.Clients.Groups(notifications[i].UserId.ToString()).SendAsync("NewNotification", notifications[i]);
            }
        }
    }
}
