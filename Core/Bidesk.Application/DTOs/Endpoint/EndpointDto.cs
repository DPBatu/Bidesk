namespace Bidesk.Application.DTOs.Endpoint
{
    public record EndpointDto
    {
        public string PortalType { get; set; } = string.Empty;
        public string MenuType { get; set; } = string.Empty;
        public string ActionType { get; set; } = string.Empty;
        public string HttpType { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
