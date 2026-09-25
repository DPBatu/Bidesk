using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Queries.TicketDefinition.GetTicketLookups
{
    public class GetTicketLookupsQHand : IRequestHandler<GetTicketLookupsQReq, GetTicketLookupsQRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public GetTicketLookupsQHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<GetTicketLookupsQRes> Handle(GetTicketLookupsQReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.GetTicketLookups(request, cancellationToken);
        }
    }
}
