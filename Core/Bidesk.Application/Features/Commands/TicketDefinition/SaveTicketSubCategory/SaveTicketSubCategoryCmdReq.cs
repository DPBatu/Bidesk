using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketSubCategory
{
    public class SaveTicketSubCategoryCmdReq : IRequest<SaveTicketSubCategoryCmdRes>
    {
        public TicketSubCategoryVM SubCategory { get; set; } = new();
    }
}
