using Hangfire;
using Bidesk.Application.Abstraction.Services.Ticket;

namespace Bidesk.Infrastructure.Hangfire
{
    public static class HangfireJobQueue
    {
        private const string AutoCloseJobId = "Çözülen Taleplerin Otomatik Kapanışı";
        private const string OverdueJobId = "Hedef Süresi Geçen Talepler";

        private static readonly string[] JobIds = { AutoCloseJobId, OverdueJobId };

        /// <summary>
        /// Bildirimler kapaliyken (Notifications:Enabled = false) daha once kaydedilmis
        /// tekrarli isleri storage'dan siler; boylece gece koşusu hic calismaz.
        /// </summary>
        public static void RemoveTasks()
        {
            foreach (var jobId in JobIds)
                RecurringJob.RemoveIfExists(jobId);
        }

        public static void CreateTasks()
        {
            RecurringJob.AddOrUpdate(AutoCloseJobId,
                (ITicketJobService jobService) => jobService.AutoCloseResolvedTicketsAsync(), Cron.Daily);

            RecurringJob.AddOrUpdate(OverdueJobId,
                (ITicketJobService jobService) => jobService.NotifyOverdueTicketsAsync(), Cron.Hourly);
        }
    }
}
