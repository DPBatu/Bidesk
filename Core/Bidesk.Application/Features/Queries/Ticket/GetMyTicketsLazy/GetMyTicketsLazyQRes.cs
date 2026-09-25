using Bidesk.Application.DTOs.User;
using Bidesk.Application.ViewModels.Ticket;

namespace Bidesk.Application.Features.Queries.Ticket.GetMyTicketsLazy
{
    public class GetMyTicketsLazyQRes : BaseRes
    {
        public List<TicketGridVM> Data { get; set; } = new();
        public long TotalRecords { get; set; }
    }
}
