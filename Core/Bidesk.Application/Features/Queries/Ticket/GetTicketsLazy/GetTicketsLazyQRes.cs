using Bidesk.Application.DTOs.User;
using Bidesk.Application.ViewModels.Ticket;

namespace Bidesk.Application.Features.Queries.Ticket.GetTicketsLazy
{
    public class GetTicketsLazyQRes : BaseRes
    {
        public List<TicketGridVM> Data { get; set; } = new();
        public long TotalRecords { get; set; }
    }
}
