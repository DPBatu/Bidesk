using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketStatuses
{
    public class GetAllTicketStatusesQHand : IRequestHandler<GetAllTicketStatusesQReq, GetAllTicketStatusesQRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public GetAllTicketStatusesQHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<GetAllTicketStatusesQRes> Handle(GetAllTicketStatusesQReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.GetAllTicketStatuses(request, cancellationToken);
        }
    }
}
