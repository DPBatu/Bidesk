using Bidesk.Application.DTOs.User;
using Bidesk.Application.ViewModels.Ticket;

namespace Bidesk.Application.Features.Queries.TicketDefinition.GetTicketLookups
{
    public class GetTicketLookupsQRes : BaseRes
    {
        public TicketLookupsVM Lookups { get; set; } = new();
    }
}
