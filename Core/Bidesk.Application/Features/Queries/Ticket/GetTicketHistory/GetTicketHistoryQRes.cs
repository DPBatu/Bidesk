using Bidesk.Application.DTOs.User;
using Bidesk.Application.ViewModels.Ticket;

namespace Bidesk.Application.Features.Queries.Ticket.GetTicketHistory
{
    public class GetTicketHistoryQRes : BaseRes
    {
        public List<TicketHistoryVM> History { get; set; } = new();
    }
}
