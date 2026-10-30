using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Queries.Ticket.GetTicketsLazy
{
    public class GetTicketsLazyQReq : IRequest<GetTicketsLazyQRes>
    {
        public string? LazyLoad { get; set; }
        public string ListType { get; set; } = "pending";
        public long? StatusID { get; set; }
        public long? PriorityID { get; set; }
        public long? GroupID { get; set; }
        public long? TechnicianUserID { get; set; }
    }
}
