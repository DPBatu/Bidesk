using Bidesk.Application.DTOs.User;
using Bidesk.Application.ViewModels.Ticket;

namespace Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketCategories
{
    public class GetAllTicketCategoriesQRes : BaseRes
    {
        public List<TicketCategoryVM> Categories { get; set; } = new();
    }
}
