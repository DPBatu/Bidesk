using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketSubCategory
{
    public class DeleteTicketSubCategoryCmdReq : IRequest<DeleteTicketSubCategoryCmdRes>
    {
        public long ID { get; set; }
    }
}
