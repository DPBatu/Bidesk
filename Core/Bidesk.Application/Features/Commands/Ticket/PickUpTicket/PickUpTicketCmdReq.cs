using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.Ticket.PickUpTicket
{
    public class PickUpTicketCmdReq : IRequest<PickUpTicketCmdRes>
    {
        public long ID { get; set; }
    }
}
