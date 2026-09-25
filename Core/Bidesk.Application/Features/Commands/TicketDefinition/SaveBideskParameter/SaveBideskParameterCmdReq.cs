using Bidesk.Application.ViewModels.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.TicketDefinition.SaveBideskParameter
{
    public class SaveBideskParameterCmdReq : IRequest<SaveBideskParameterCmdRes>
    {
        public BideskParameterVM Parameter { get; set; } = new();
    }
}
