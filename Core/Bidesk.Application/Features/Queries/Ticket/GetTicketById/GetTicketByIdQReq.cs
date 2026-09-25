using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Queries.Ticket.GetTicketById
{
    public class GetTicketByIdQReq : IRequest<GetTicketByIdQRes>
    {
        public long ID { get; set; }
    }
}
