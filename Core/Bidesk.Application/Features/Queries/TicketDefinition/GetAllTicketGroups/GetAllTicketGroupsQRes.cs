using Bidesk.Application.DTOs.User;
using Bidesk.Application.ViewModels.Ticket;

namespace Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketGroups
{
    public class GetAllTicketGroupsQRes : BaseRes
    {
        public List<TicketGroupVM> Groups { get; set; } = new();
    }
}
