using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.Ticket.UpdateTicketField
{
    public class UpdateTicketFieldCmdHand : IRequestHandler<UpdateTicketFieldCmdReq, UpdateTicketFieldCmdRes>
    {
        readonly ITicketService _ticketService;

        public UpdateTicketFieldCmdHand(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<UpdateTicketFieldCmdRes> Handle(UpdateTicketFieldCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketService.UpdateTicketField(request, cancellationToken);
        }
    }
}
