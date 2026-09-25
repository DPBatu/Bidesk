using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Queries.Ticket.GetTicketHistory
{
    public class GetTicketHistoryQHand : IRequestHandler<GetTicketHistoryQReq, GetTicketHistoryQRes>
    {
        readonly ITicketService _ticketService;

        public GetTicketHistoryQHand(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<GetTicketHistoryQRes> Handle(GetTicketHistoryQReq request, CancellationToken cancellationToken)
        {
            return await _ticketService.GetTicketHistory(request, cancellationToken);
        }
    }
}
