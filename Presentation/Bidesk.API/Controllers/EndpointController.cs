using Bidesk.Application.Abstraction.Services;
using Bidesk.Application.Custom_Attributes;
using Bidesk.Application.Enums.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bidesk.API.Controllers
{
    [Route("api/bidesk/[controller]")]
    [ApiController]
    public class EndpointController : ControllerBase
    {
        readonly IEndpointService _endpointService;

        public EndpointController(IEndpointService endpointService)
        {
            _endpointService = endpointService;
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Loggedin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get API Endpoints", Menu = MenuType.Roles, Portal = PortalType.Bidesk)]
        public IActionResult GetAPIEndpoints()
        {
            return Ok(_endpointService.GetAPIEndpoints(typeof(Program)));
        }
    }
}
