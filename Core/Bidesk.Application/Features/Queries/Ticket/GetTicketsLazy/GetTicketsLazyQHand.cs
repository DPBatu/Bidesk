using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Queries.Ticket.GetTicketsLazy
{
    public class GetTicketsLazyQHand : IRequestHandler<GetTicketsLazyQReq, GetTicketsLazyQRes>
    {
        readonly ITicketService _ticketService;

        public GetTicketsLazyQHand(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<GetTicketsLazyQRes> Handle(GetTicketsLazyQReq request, CancellationToken cancellationToken)
        {
            return await _ticketService.GetTicketsLazy(request, cancellationToken);
        }
    }
}
