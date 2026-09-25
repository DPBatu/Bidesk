using Bidesk.Application.DTOs.User;
using Bidesk.Application.ViewModels.Ticket;

namespace Bidesk.Application.Features.Queries.TicketDefinition.GetBideskParameters
{
    public class GetBideskParametersQRes : BaseRes
    {
        public List<BideskParameterVM> Parameters { get; set; } = new();
    }
}
