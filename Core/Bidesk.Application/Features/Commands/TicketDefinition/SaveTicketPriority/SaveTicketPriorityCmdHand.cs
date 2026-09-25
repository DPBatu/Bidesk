using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketPriority
{
    public class SaveTicketPriorityCmdHand : IRequestHandler<SaveTicketPriorityCmdReq, SaveTicketPriorityCmdRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public SaveTicketPriorityCmdHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<SaveTicketPriorityCmdRes> Handle(SaveTicketPriorityCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.SaveTicketPriority(request, cancellationToken);
        }
    }
}
