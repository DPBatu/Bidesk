using Bidesk.Application.DTOs.User;
using Bidesk.Application.ViewModels.Ticket;

namespace Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketStatuses
{
    public class GetAllTicketStatusesQRes : BaseRes
    {
        public List<TicketStatusVM> Statuses { get; set; } = new();
    }
}
