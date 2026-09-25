using Bidesk.Application.DTOs.User;
using Bidesk.Application.ViewModels.Ticket;

namespace Bidesk.Application.Features.Queries.Ticket.GetTicketDashboard
{
    public class GetTicketDashboardQRes : BaseRes
    {
        public TicketDashboardVM Dashboard { get; set; } = new();
    }
}
