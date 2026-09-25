using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketPriorities
{
    public class GetAllTicketPrioritiesQHand : IRequestHandler<GetAllTicketPrioritiesQReq, GetAllTicketPrioritiesQRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public GetAllTicketPrioritiesQHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<GetAllTicketPrioritiesQRes> Handle(GetAllTicketPrioritiesQReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.GetAllTicketPriorities(request, cancellationToken);
        }
    }
}
