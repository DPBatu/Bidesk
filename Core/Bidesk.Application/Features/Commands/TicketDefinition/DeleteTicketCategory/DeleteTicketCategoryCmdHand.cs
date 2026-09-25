using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketCategory
{
    public class DeleteTicketCategoryCmdHand : IRequestHandler<DeleteTicketCategoryCmdReq, DeleteTicketCategoryCmdRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public DeleteTicketCategoryCmdHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<DeleteTicketCategoryCmdRes> Handle(DeleteTicketCategoryCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.DeleteTicketCategory(request, cancellationToken);
        }
    }
}
