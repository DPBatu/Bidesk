using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.Ticket.AddTicketConversation
{
    public class AddTicketConversationCmdReq : IRequest<AddTicketConversationCmdRes>
    {
        public long TicketID { get; set; }
        public string Body { get; set; } = string.Empty;
        public bool IsInternal { get; set; }
        public long? StatusID { get; set; }
        public List<TicketAttachmentVM> Attachments { get; set; } = new();
    }
}
