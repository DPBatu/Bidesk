using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.Ticket.DeleteTicketAttachment
{
    public class DeleteTicketAttachmentCmdReq : IRequest<DeleteTicketAttachmentCmdRes>
    {
        public long ID { get; set; }
    }
}
