using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketPriority
{
    public class DeleteTicketPriorityCmdHand : IRequestHandler<DeleteTicketPriorityCmdReq, DeleteTicketPriorityCmdRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public DeleteTicketPriorityCmdHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<DeleteTicketPriorityCmdRes> Handle(DeleteTicketPriorityCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.DeleteTicketPriority(request, cancellationToken);
        }
    }
}
