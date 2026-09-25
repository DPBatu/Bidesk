using Bidesk.Application.Custom_Attributes;
using Bidesk.Application.Features.Commands.Ticket.AddTicketConversation;
using Bidesk.Application.Features.Commands.Ticket.CreateTicket;
using Bidesk.Application.Features.Commands.Ticket.DeleteTicketAttachment;
using Bidesk.Application.Features.Commands.Ticket.PickUpTicket;
using Bidesk.Application.Features.Commands.Ticket.SaveTicketAttachment;
using Bidesk.Application.Features.Commands.Ticket.SaveTicketResolution;
using Bidesk.Application.Features.Commands.Ticket.UpdateTicket;
using Bidesk.Application.Features.Commands.Ticket.UpdateTicketField;
using Bidesk.Application.Features.Queries.Ticket.GetMyTicketsLazy;
using Bidesk.Application.Features.Queries.Ticket.GetTicketById;
using Bidesk.Application.Features.Queries.Ticket.GetTicketDashboard;
using Bidesk.Application.Features.Queries.Ticket.GetTicketHistory;
using Bidesk.Application.Features.Queries.Ticket.GetTicketsLazy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bidesk.API.Controllers
{
    /// <summary>
    /// Talep uclari. Her calisan talep acabildigi icin rol izni aranmaz ([AllowBypassFilter]);
    /// kimlik dogrulama zorunludur ve kim neyi gorur/degistirir kurali TicketService'te
    /// (istek sahibi / bildirim listesi / grup teknisyeni / admin) uygulanir.
    /// </summary>
    [Route("api/bidesk/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Loggedin")]
    public class TicketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region GET

        [HttpGet("[action]")]
        [AllowBypassFilter]
        public async Task<IActionResult> GetMyTicketsLazy([FromQuery] GetMyTicketsLazyQReq req)
            => Ok(await _mediator.Send(req));

        [HttpGet("[action]")]
        [AllowBypassFilter]
        public async Task<IActionResult> GetTicketsLazy([FromQuery] GetTicketsLazyQReq req)
            => Ok(await _mediator.Send(req));

        [HttpGet("[action]")]
        [AllowBypassFilter]
        public async Task<IActionResult> GetTicketById([FromQuery] GetTicketByIdQReq req)
            => Ok(await _mediator.Send(req));

        [HttpGet("[action]")]
        [AllowBypassFilter]
        public async Task<IActionResult> GetTicketHistory([FromQuery] GetTicketHistoryQReq req)
            => Ok(await _mediator.Send(req));

        [HttpGet("[action]")]
        [AllowBypassFilter]
        public async Task<IActionResult> GetTicketDashboard([FromQuery] GetTicketDashboardQReq req)
            => Ok(await _mediator.Send(req));

        #endregion

        #region POST

        [HttpPost("[action]")]
        [AllowBypassFilter]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketCmdReq req)
            => Ok(await _mediator.Send(req));

        [HttpPost("[action]")]
        [AllowBypassFilter]
        public async Task<IActionResult> UpdateTicket([FromBody] UpdateTicketCmdReq req)
            => Ok(await _mediator.Send(req));

        [HttpPost("[action]")]
        [AllowBypassFilter]
        public async Task<IActionResult> UpdateTicketField([FromBody] UpdateTicketFieldCmdReq req)
            => Ok(await _mediator.Send(req));

        [HttpPost("[action]")]
        [AllowBypassFilter]
        public async Task<IActionResult> PickUpTicket([FromBody] PickUpTicketCmdReq req)
            => Ok(await _mediator.Send(req));

        [HttpPost("[action]")]
        [AllowBypassFilter]
        public async Task<IActionResult> AddTicketConversation([FromBody] AddTicketConversationCmdReq req)
            => Ok(await _mediator.Send(req));

        [HttpPost("[action]")]
        [AllowBypassFilter]
        public async Task<IActionResult> SaveTicketResolution([FromBody] SaveTicketResolutionCmdReq req)
            => Ok(await _mediator.Send(req));

        [HttpPost("[action]")]
        [AllowBypassFilter]
        public async Task<IActionResult> SaveTicketAttachment([FromBody] SaveTicketAttachmentCmdReq req)
            => Ok(await _mediator.Send(req));

        [HttpPost("[action]")]
        [AllowBypassFilter]
        public async Task<IActionResult> DeleteTicketAttachment([FromBody] DeleteTicketAttachmentCmdReq req)
            => Ok(await _mediator.Send(req));

        #endregion
    }
}
