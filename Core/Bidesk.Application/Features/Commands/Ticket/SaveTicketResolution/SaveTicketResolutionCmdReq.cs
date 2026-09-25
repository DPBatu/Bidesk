using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.Ticket.SaveTicketResolution
{
    public class SaveTicketResolutionCmdReq : IRequest<SaveTicketResolutionCmdRes>
    {
        public long ID { get; set; }
        public string? Resolution { get; set; }
        public bool MarkResolved { get; set; }
    }
}
