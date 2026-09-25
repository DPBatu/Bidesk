using Bidesk.Application.DTOs.User;
using Bidesk.Application.ViewModels.Ticket;

namespace Bidesk.Application.Features.Queries.Ticket.GetTicketById
{
    public class GetTicketByIdQRes : BaseRes
    {
        public TicketDetailVM? Ticket { get; set; }
    }
}
