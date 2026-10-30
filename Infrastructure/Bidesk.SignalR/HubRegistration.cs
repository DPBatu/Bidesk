using Microsoft.AspNetCore.Builder;

namespace Bidesk.SignalR
{
    public static class HubRegistration
    {
        public static void RegisterHubs(this WebApplication webApplication)
        {
            webApplication.MapHub<NotificationHub>("api/bidesk/hubs/notification");
        }
    }
}
