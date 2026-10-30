using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.Ticket.UpdateTicketField
{
    public class UpdateTicketFieldCmdReq : IRequest<UpdateTicketFieldCmdRes>
    {
        public long ID { get; set; }
        public string Field { get; set; } = string.Empty;
        public long? Value { get; set; }
    }
}
