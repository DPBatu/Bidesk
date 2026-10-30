using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.Ticket.PickUpTicket
{
    public class PickUpTicketCmdHand : IRequestHandler<PickUpTicketCmdReq, PickUpTicketCmdRes>
    {
        readonly ITicketService _ticketService;

        public PickUpTicketCmdHand(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<PickUpTicketCmdRes> Handle(PickUpTicketCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketService.PickUpTicket(request, cancellationToken);
        }
    }
}
