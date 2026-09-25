using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Queries.Ticket.GetTicketHistory
{
    public class GetTicketHistoryQReq : IRequest<GetTicketHistoryQRes>
    {
        public long ID { get; set; }
    }
}
