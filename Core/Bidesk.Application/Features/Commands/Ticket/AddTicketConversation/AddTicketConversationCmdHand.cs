using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.Ticket.AddTicketConversation
{
    public class AddTicketConversationCmdHand : IRequestHandler<AddTicketConversationCmdReq, AddTicketConversationCmdRes>
    {
        readonly ITicketService _ticketService;

        public AddTicketConversationCmdHand(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<AddTicketConversationCmdRes> Handle(AddTicketConversationCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketService.AddTicketConversation(request, cancellationToken);
        }
    }
}
