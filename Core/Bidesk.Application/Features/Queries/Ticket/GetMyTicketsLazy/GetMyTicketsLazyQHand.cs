using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Queries.Ticket.GetMyTicketsLazy
{
    public class GetMyTicketsLazyQHand : IRequestHandler<GetMyTicketsLazyQReq, GetMyTicketsLazyQRes>
    {
        readonly ITicketService _ticketService;

        public GetMyTicketsLazyQHand(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<GetMyTicketsLazyQRes> Handle(GetMyTicketsLazyQReq request, CancellationToken cancellationToken)
        {
            return await _ticketService.GetMyTicketsLazy(request, cancellationToken);
        }
    }
}
