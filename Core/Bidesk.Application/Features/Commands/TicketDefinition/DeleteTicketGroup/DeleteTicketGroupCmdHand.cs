using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketGroup
{
    public class DeleteTicketGroupCmdHand : IRequestHandler<DeleteTicketGroupCmdReq, DeleteTicketGroupCmdRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public DeleteTicketGroupCmdHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<DeleteTicketGroupCmdRes> Handle(DeleteTicketGroupCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.DeleteTicketGroup(request, cancellationToken);
        }
    }
}
