namespace Bidesk.Domain.Entities.Public
{
    public class EndpointEnt: BaseEntity
    {
        public string PortalName { get; set; } = string.Empty;
        public string HttpType { get; set; } = string.Empty;
        public string ActionType { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public MenuEnt Menu { get; set; } = new();
        public ICollection<AppRoleEnt> Roles { get; set; } = new List<AppRoleEnt>();
    }
}
