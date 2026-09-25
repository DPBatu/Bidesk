using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Queries.Ticket.GetTicketById
{
    public class GetTicketByIdQHand : IRequestHandler<GetTicketByIdQReq, GetTicketByIdQRes>
    {
        readonly ITicketService _ticketService;

        public GetTicketByIdQHand(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<GetTicketByIdQRes> Handle(GetTicketByIdQReq request, CancellationToken cancellationToken)
        {
            return await _ticketService.GetTicketById(request, cancellationToken);
        }
    }
}
