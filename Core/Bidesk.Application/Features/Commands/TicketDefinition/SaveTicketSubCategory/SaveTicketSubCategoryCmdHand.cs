using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketSubCategory
{
    public class SaveTicketSubCategoryCmdHand : IRequestHandler<SaveTicketSubCategoryCmdReq, SaveTicketSubCategoryCmdRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public SaveTicketSubCategoryCmdHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<SaveTicketSubCategoryCmdRes> Handle(SaveTicketSubCategoryCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.SaveTicketSubCategory(request, cancellationToken);
        }
    }
}
