using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketStatus
{
    public class SaveTicketStatusCmdHand : IRequestHandler<SaveTicketStatusCmdReq, SaveTicketStatusCmdRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public SaveTicketStatusCmdHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<SaveTicketStatusCmdRes> Handle(SaveTicketStatusCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.SaveTicketStatus(request, cancellationToken);
        }
    }
}
