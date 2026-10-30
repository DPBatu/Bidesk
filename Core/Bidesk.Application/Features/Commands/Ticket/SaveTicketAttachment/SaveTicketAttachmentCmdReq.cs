using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.Ticket.SaveTicketAttachment
{
    public class SaveTicketAttachmentCmdReq : IRequest<SaveTicketAttachmentCmdRes>
    {
        public long TicketID { get; set; }
        public List<TicketAttachmentVM> Attachments { get; set; } = new();
    }
}
