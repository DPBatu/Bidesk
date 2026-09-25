using Bidesk.Application.DTOs.User;
using Bidesk.Application.ViewModels.Ticket;

namespace Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketPriorities
{
    public class GetAllTicketPrioritiesQRes : BaseRes
    {
        public List<TicketPriorityVM> Priorities { get; set; } = new();
    }
}
