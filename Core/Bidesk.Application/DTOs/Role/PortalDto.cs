namespace Bidesk.Application.DTOs.Role
{
    public class PortalDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public List<ActionDto>? Actions { get; set; } = new List<ActionDto>();
    }
}
