using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketPriority
{
    public class SaveTicketPriorityCmdReq : IRequest<SaveTicketPriorityCmdRes>
    {
        public TicketPriorityVM Priority { get; set; } = new();
    }
}
