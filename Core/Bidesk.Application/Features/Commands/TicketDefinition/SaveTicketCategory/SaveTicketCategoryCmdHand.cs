using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketCategory
{
    public class SaveTicketCategoryCmdHand : IRequestHandler<SaveTicketCategoryCmdReq, SaveTicketCategoryCmdRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public SaveTicketCategoryCmdHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<SaveTicketCategoryCmdRes> Handle(SaveTicketCategoryCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.SaveTicketCategory(request, cancellationToken);
        }
    }
}
