using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketStatus
{
    public class DeleteTicketStatusCmdHand : IRequestHandler<DeleteTicketStatusCmdReq, DeleteTicketStatusCmdRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public DeleteTicketStatusCmdHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<DeleteTicketStatusCmdRes> Handle(DeleteTicketStatusCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.DeleteTicketStatus(request, cancellationToken);
        }
    }
}
