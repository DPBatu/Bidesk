using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Queries.TicketDefinition.GetBideskParameters
{
    public class GetBideskParametersQHand : IRequestHandler<GetBideskParametersQReq, GetBideskParametersQRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public GetBideskParametersQHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<GetBideskParametersQRes> Handle(GetBideskParametersQReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.GetBideskParameters(request, cancellationToken);
        }
    }
}
