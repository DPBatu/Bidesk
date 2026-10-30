using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketPriority
{
    public class DeleteTicketPriorityCmdReq : IRequest<DeleteTicketPriorityCmdRes>
    {
        public long ID { get; set; }
    }
}
