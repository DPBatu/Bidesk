using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.Ticket.SaveTicketResolution
{
    public class SaveTicketResolutionCmdHand : IRequestHandler<SaveTicketResolutionCmdReq, SaveTicketResolutionCmdRes>
    {
        readonly ITicketService _ticketService;

        public SaveTicketResolutionCmdHand(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<SaveTicketResolutionCmdRes> Handle(SaveTicketResolutionCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketService.SaveTicketResolution(request, cancellationToken);
        }
    }
}
