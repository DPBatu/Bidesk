using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.Ticket.CreateTicket
{
    public class CreateTicketCmdHand : IRequestHandler<CreateTicketCmdReq, CreateTicketCmdRes>
    {
        readonly ITicketService _ticketService;

        public CreateTicketCmdHand(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<CreateTicketCmdRes> Handle(CreateTicketCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketService.CreateTicket(request, cancellationToken);
        }
    }
}
