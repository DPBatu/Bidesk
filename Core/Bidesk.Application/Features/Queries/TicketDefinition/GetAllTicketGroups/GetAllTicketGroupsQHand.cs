using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketGroups
{
    public class GetAllTicketGroupsQHand : IRequestHandler<GetAllTicketGroupsQReq, GetAllTicketGroupsQRes>
    {
        readonly ITicketDefinitionService _ticketDefinitionService;

        public GetAllTicketGroupsQHand(ITicketDefinitionService ticketDefinitionService)
        {
            _ticketDefinitionService = ticketDefinitionService;
        }

        public async Task<GetAllTicketGroupsQRes> Handle(GetAllTicketGroupsQReq request, CancellationToken cancellationToken)
        {
            return await _ticketDefinitionService.GetAllTicketGroups(request, cancellationToken);
        }
    }
}
