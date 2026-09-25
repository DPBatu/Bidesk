using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketCategories
{
    public class GetAllTicketCategoriesQHand : IRequestHandler<GetAllTicketCategoriesQReq, GetAllTicketCategoriesQRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public GetAllTicketCategoriesQHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<GetAllTicketCategoriesQRes> Handle(GetAllTicketCategoriesQReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.GetAllTicketCategories(request, cancellationToken);
        }
    }
}
