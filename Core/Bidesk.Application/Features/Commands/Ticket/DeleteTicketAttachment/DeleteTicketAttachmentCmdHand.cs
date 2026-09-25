using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.Ticket.DeleteTicketAttachment
{
    public class DeleteTicketAttachmentCmdHand : IRequestHandler<DeleteTicketAttachmentCmdReq, DeleteTicketAttachmentCmdRes>
    {
        readonly ITicketService _ticketService;

        public DeleteTicketAttachmentCmdHand(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<DeleteTicketAttachmentCmdRes> Handle(DeleteTicketAttachmentCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketService.DeleteTicketAttachment(request, cancellationToken);
        }
    }
}
