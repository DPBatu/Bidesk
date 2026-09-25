using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketCategory
{
    public class DeleteTicketCategoryCmdReq : IRequest<DeleteTicketCategoryCmdRes>
    {
        public long ID { get; set; }
    }
}
