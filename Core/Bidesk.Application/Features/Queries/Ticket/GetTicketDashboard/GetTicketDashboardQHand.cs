using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Queries.Ticket.GetTicketDashboard
{
    public class GetTicketDashboardQHand : IRequestHandler<GetTicketDashboardQReq, GetTicketDashboardQRes>
    {
        readonly ITicketService _ticketService;

        public GetTicketDashboardQHand(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<GetTicketDashboardQRes> Handle(GetTicketDashboardQReq request, CancellationToken cancellationToken)
        {
            return await _ticketService.GetTicketDashboard(request, cancellationToken);
        }
    }
}
