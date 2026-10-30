using Bidesk.Application.Custom_Attributes;
using Bidesk.Application.Enums.User;
using Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketCategory;
using Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketGroup;
using Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketPriority;
using Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketStatus;
using Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketSubCategory;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveBideskParameter;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketCategory;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketGroup;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketPriority;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketStatus;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketSubCategory;
using Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketCategories;
using Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketGroups;
using Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketPriorities;
using Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketStatuses;
using Bidesk.Application.Features.Queries.TicketDefinition.GetBideskParameters;
using Bidesk.Application.Features.Queries.TicketDefinition.GetBideskPersonnel;
using Bidesk.Application.Features.Queries.TicketDefinition.GetTicketLookups;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bidesk.API.Controllers
{
    /// <summary>
    /// Tanim tablolari. Form icin gereken aktif tanimlar ve personel listesi herkese acik;
    /// tanim yonetimi Portal'daki rol izniyle (MenuType.BideskAdmin) korunur.
    /// </summary>
    [Route("api/bidesk/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Loggedin")]
    public class TicketDefinitionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketDefinitionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Herkese acik

        [HttpGet("[action]")]
        [AllowBypassFilter]
        public async Task<IActionResult> GetTicketLookups([FromQuery] GetTicketLookupsQReq req)
            => Ok(await _mediator.Send(req));

        [HttpGet("[action]")]
        [AllowBypassFilter]
        public async Task<IActionResult> GetBideskPersonnel([FromQuery] GetBideskPersonnelQReq req)
            => Ok(await _mediator.Send(req));

        #endregion

        #region Yonetim - GET

        [HttpGet("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Ticket Statuses", Menu = MenuType.BideskAdmin, Portal = PortalType.Bidesk)]
        public async Task<IActionResult> GetAllTicketStatuses([FromQuery] GetAllTicketStatusesQReq req)
            => Ok(await _mediator.Send(req));

        [HttpGet("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Ticket Priorities", Menu = MenuType.BideskAdmin, Portal = PortalType.Bidesk)]
        public async Task<IActionResult> GetAllTicketPriorities([FromQuery] GetAllTicketPrioritiesQReq req)
            => Ok(await _mediator.Send(req));

        [HttpGet("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Ticket Groups", Menu = MenuType.BideskAdmin, Portal = PortalType.Bidesk)]
        public async Task<IActionResult> GetAllTicketGroups([FromQuery] GetAllTicketGroupsQReq req)
            => Ok(await _mediator.Send(req));

        [HttpGet("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Ticket Categories", Menu = MenuType.BideskAdmin, Portal = PortalType.Bidesk)]
        public async Task<IActionResult> GetAllTicketCategories([FromQuery] GetAllTicketCategoriesQReq req)
            => Ok(await _mediator.Send(req));

        [HttpGet("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Bidesk Parameters", Menu = MenuType.BideskAdmin, Portal = PortalType.Bidesk)]
        public async Task<IActionResult> GetBideskParameters([FromQuery] GetBideskParametersQReq req)
            => Ok(await _mediator.Send(req));

        #endregion

        #region Yonetim - POST

        [HttpPost("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Save Ticket Status", Menu = MenuType.BideskAdmin, Portal = PortalType.Bidesk)]
        public async Task<IActionResult> SaveTicketStatus([FromBody] SaveTicketStatusCmdReq req)
            => Ok(await _mediator.Send(req));

        [HttpPost("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Deleting, Definition = "Delete Ticket Status", Menu = MenuType.BideskAdmin, Portal = PortalType.Bidesk)]
        public async Task<IActionResult> DeleteTicketStatus([FromBody] DeleteTicketStatusCmdReq req)
            => Ok(await _mediator.Send(req));

        [HttpPost("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Save Ticket Priority", Menu = MenuType.BideskAdmin, Portal = PortalType.Bidesk)]
        public async Task<IActionResult> SaveTicketPriority([FromBody] SaveTicketPriorityCmdReq req)
            => Ok(await _mediator.Send(req));

        [HttpPost("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Deleting, Definition = "Delete Ticket Priority", Menu = MenuType.BideskAdmin, Portal = PortalType.Bidesk)]
        public async Task<IActionResult> DeleteTicketPriority([FromBody] DeleteTicketPriorityCmdReq req)
            => Ok(await _mediator.Send(req));

        [HttpPost("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Save Ticket Group", Menu = MenuType.BideskAdmin, Portal = PortalType.Bidesk)]
        public async Task<IActionResult> SaveTicketGroup([FromBody] SaveTicketGroupCmdReq req)
            => Ok(await _mediator.Send(req));

        [HttpPost("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Deleting, Definition = "Delete Ticket Group", Menu = MenuType.BideskAdmin, Portal = PortalType.Bidesk)]
        public async Task<IActionResult> DeleteTicketGroup([FromBody] DeleteTicketGroupCmdReq req)
            => Ok(await _mediator.Send(req));

        [HttpPost("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Save Ticket Category", Menu = MenuType.BideskAdmin, Portal = PortalType.Bidesk)]
        public async Task<IActionResult> SaveTicketCategory([FromBody] SaveTicketCategoryCmdReq req)
            => Ok(await _mediator.Send(req));

        [HttpPost("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Deleting, Definition = "Delete Ticket Category", Menu = MenuType.BideskAdmin, Portal = PortalType.Bidesk)]
        public async Task<IActionResult> DeleteTicketCategory([FromBody] DeleteTicketCategoryCmdReq req)
            => Ok(await _mediator.Send(req));

        [HttpPost("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Save Ticket SubCategory", Menu = MenuType.BideskAdmin, Portal = PortalType.Bidesk)]
        public async Task<IActionResult> SaveTicketSubCategory([FromBody] SaveTicketSubCategoryCmdReq req)
            => Ok(await _mediator.Send(req));

        [HttpPost("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Deleting, Definition = "Delete Ticket SubCategory", Menu = MenuType.BideskAdmin, Portal = PortalType.Bidesk)]
        public async Task<IActionResult> DeleteTicketSubCategory([FromBody] DeleteTicketSubCategoryCmdReq req)
            => Ok(await _mediator.Send(req));

        [HttpPost("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Save Bidesk Parameter", Menu = MenuType.BideskAdmin, Portal = PortalType.Bidesk)]
        public async Task<IActionResult> SaveBideskParameter([FromBody] SaveBideskParameterCmdReq req)
            => Ok(await _mediator.Send(req));

        #endregion
    }
}
