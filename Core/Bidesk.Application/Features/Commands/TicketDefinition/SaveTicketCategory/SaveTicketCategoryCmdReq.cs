using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketCategory
{
    public class SaveTicketCategoryCmdReq : IRequest<SaveTicketCategoryCmdRes>
    {
        public TicketCategoryVM Category { get; set; } = new();
    }
}
