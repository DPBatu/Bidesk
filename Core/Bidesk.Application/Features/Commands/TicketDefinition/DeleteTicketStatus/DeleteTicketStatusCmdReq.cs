using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketStatus
{
    public class DeleteTicketStatusCmdReq : IRequest<DeleteTicketStatusCmdRes>
    {
        public long ID { get; set; }
    }
}
