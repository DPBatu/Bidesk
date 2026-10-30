using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketSubCategory
{
    public class DeleteTicketSubCategoryCmdHand : IRequestHandler<DeleteTicketSubCategoryCmdReq, DeleteTicketSubCategoryCmdRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public DeleteTicketSubCategoryCmdHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<DeleteTicketSubCategoryCmdRes> Handle(DeleteTicketSubCategoryCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.DeleteTicketSubCategory(request, cancellationToken);
        }
    }
}
