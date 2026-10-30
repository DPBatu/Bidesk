using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.Ticket.UpdateTicket
{
    public class UpdateTicketCmdReq : IRequest<UpdateTicketCmdRes>
    {
        public long ID { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long StatusID { get; set; }
        public long PriorityID { get; set; }
        public long GroupID { get; set; }
        public long? TechnicianUserID { get; set; }
        public long? CategoryID { get; set; }
        public long? SubCategoryID { get; set; }
        public List<long> NotifyUserIDs { get; set; } = new();
    }
}
