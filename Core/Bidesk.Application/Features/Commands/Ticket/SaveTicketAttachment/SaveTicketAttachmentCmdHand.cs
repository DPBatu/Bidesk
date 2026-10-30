using Bidesk.Application.Abstraction.Services.Ticket;
using MediatR;

namespace Bidesk.Application.Features.Commands.Ticket.SaveTicketAttachment
{
    public class SaveTicketAttachmentCmdHand : IRequestHandler<SaveTicketAttachmentCmdReq, SaveTicketAttachmentCmdRes>
    {
        readonly ITicketService _ticketService;

        public SaveTicketAttachmentCmdHand(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<SaveTicketAttachmentCmdRes> Handle(SaveTicketAttachmentCmdReq request, CancellationToken cancellationToken)
        {
            return await _ticketService.SaveTicketAttachment(request, cancellationToken);
        }
    }
}
