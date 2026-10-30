using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Queries.Ticket.GetMyTicketsLazy
{
    public class GetMyTicketsLazyQReq : IRequest<GetMyTicketsLazyQRes>
    {
        public string? LazyLoad { get; set; }
        public bool IncludeClosed { get; set; }
    }
}
