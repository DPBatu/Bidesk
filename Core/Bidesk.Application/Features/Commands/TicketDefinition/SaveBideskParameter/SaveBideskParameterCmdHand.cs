using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.SaveBideskParameter
{
    public class SaveBideskParameterCmdHand : IRequestHandler<SaveBideskParameterCmdReq, SaveBideskParameterCmdRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public SaveBideskParameterCmdHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<SaveBideskParameterCmdRes> Handle(SaveBideskParameterCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.SaveBideskParameter(request, cancellationToken);
        }
    }
}
