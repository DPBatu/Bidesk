using Bidesk.Application.Features.Queries.Ticket.GetMyTicketsLazy;
using Bidesk.Application.Features.Queries.Ticket.GetTicketsLazy;
using Bidesk.Application.Features.Queries.Ticket.GetTicketById;
using Bidesk.Application.Features.Queries.Ticket.GetTicketHistory;
using Bidesk.Application.Features.Queries.Ticket.GetTicketDashboard;
using Bidesk.Application.Features.Commands.Ticket.CreateTicket;
using Bidesk.Application.Features.Commands.Ticket.UpdateTicket;
using Bidesk.Application.Features.Commands.Ticket.UpdateTicketField;
using Bidesk.Application.Features.Commands.Ticket.PickUpTicket;
using Bidesk.Application.Features.Commands.Ticket.AddTicketConversation;
using Bidesk.Application.Features.Commands.Ticket.SaveTicketResolution;
using Bidesk.Application.Features.Commands.Ticket.SaveTicketAttachment;
using Bidesk.Application.Features.Commands.Ticket.DeleteTicketAttachment;

namespace Bidesk.Application.Abstraction.Services.Ticket
{
    /// <summary>Talep yasam dongusu: acma, listeleme, atama, konusma, cozumleme.</summary>
    public interface ITicketService
    {
        Task<GetMyTicketsLazyQRes> GetMyTicketsLazy(GetMyTicketsLazyQReq request, CancellationToken cancellationToken);
        Task<GetTicketsLazyQRes> GetTicketsLazy(GetTicketsLazyQReq request, CancellationToken cancellationToken);
        Task<GetTicketByIdQRes> GetTicketById(GetTicketByIdQReq request, CancellationToken cancellationToken);
        Task<GetTicketHistoryQRes> GetTicketHistory(GetTicketHistoryQReq request, CancellationToken cancellationToken);
        Task<GetTicketDashboardQRes> GetTicketDashboard(GetTicketDashboardQReq request, CancellationToken cancellationToken);
        Task<CreateTicketCmdRes> CreateTicket(CreateTicketCmdReq request, CancellationToken cancellationToken);
        Task<UpdateTicketCmdRes> UpdateTicket(UpdateTicketCmdReq request, CancellationToken cancellationToken);
        Task<UpdateTicketFieldCmdRes> UpdateTicketField(UpdateTicketFieldCmdReq request, CancellationToken cancellationToken);
        Task<PickUpTicketCmdRes> PickUpTicket(PickUpTicketCmdReq request, CancellationToken cancellationToken);
        Task<AddTicketConversationCmdRes> AddTicketConversation(AddTicketConversationCmdReq request, CancellationToken cancellationToken);
        Task<SaveTicketResolutionCmdRes> SaveTicketResolution(SaveTicketResolutionCmdReq request, CancellationToken cancellationToken);
        Task<SaveTicketAttachmentCmdRes> SaveTicketAttachment(SaveTicketAttachmentCmdReq request, CancellationToken cancellationToken);
        Task<DeleteTicketAttachmentCmdRes> DeleteTicketAttachment(DeleteTicketAttachmentCmdReq request, CancellationToken cancellationToken);
    }
}
