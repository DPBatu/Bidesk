using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketStatus
{
    public class SaveTicketStatusCmdReq : IRequest<SaveTicketStatusCmdRes>
    {
        public TicketStatusVM Status { get; set; } = new();
    }
}
