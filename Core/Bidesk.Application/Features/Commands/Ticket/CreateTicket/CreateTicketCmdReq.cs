using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.Ticket.CreateTicket
{
    public class CreateTicketCmdReq : IRequest<CreateTicketCmdRes>
    {
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long GroupID { get; set; }
        public long? PriorityID { get; set; }
        public long? CategoryID { get; set; }
        public long? SubCategoryID { get; set; }
        public List<long> NotifyUserIDs { get; set; } = new();
        public List<TicketAttachmentVM> Attachments { get; set; } = new();
    }
}
