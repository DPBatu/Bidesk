using Bidesk.Application.DTOs.User;
using Bidesk.Application.ViewModels.Ticket;

namespace Bidesk.Application.Features.Queries.TicketDefinition.GetBideskPersonnel
{
    public class GetBideskPersonnelQRes : BaseRes
    {
        public List<PersonnelDto> Personnel { get; set; } = new();
    }
}
