using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.Ticket.UpdateTicket
{
    public class UpdateTicketCmdHand : IRequestHandler<UpdateTicketCmdReq, UpdateTicketCmdRes>
    {
        readonly ITicketService _ticketService;

        public UpdateTicketCmdHand(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<UpdateTicketCmdRes> Handle(UpdateTicketCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketService.UpdateTicket(request, cancellationToken);
        }
    }
}
