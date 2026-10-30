using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketGroup
{
    public class SaveTicketGroupCmdHand : IRequestHandler<SaveTicketGroupCmdReq, SaveTicketGroupCmdRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public SaveTicketGroupCmdHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<SaveTicketGroupCmdRes> Handle(SaveTicketGroupCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.SaveTicketGroup(request, cancellationToken);
        }
    }
}
