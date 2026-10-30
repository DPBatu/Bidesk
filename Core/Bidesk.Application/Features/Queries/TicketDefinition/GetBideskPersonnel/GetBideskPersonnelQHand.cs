using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Queries.TicketDefinition.GetBideskPersonnel
{
    public class GetBideskPersonnelQHand : IRequestHandler<GetBideskPersonnelQReq, GetBideskPersonnelQRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public GetBideskPersonnelQHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<GetBideskPersonnelQRes> Handle(GetBideskPersonnelQReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.GetBideskPersonnel(request, cancellationToken);
        }
    }
}
