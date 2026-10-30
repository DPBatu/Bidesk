using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketGroup
{
    public class SaveTicketGroupCmdReq : IRequest<SaveTicketGroupCmdRes>
    {
        public TicketGroupVM Group { get; set; } = new();
    }
}
