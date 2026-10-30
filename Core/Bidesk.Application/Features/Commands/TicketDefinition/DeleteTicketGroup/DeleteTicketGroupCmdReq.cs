using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketGroup
{
    public class DeleteTicketGroupCmdReq : IRequest<DeleteTicketGroupCmdRes>
    {
        public long ID { get; set; }
    }
}
