using Bidesk.Application.Enums.User;

namespace Bidesk.Application.Custom_Attributes
{
    public class AuthorizeDefinitionAttribute : Attribute
    {
        public MenuType Menu { get; set; }
        public string Definition { get; set; } = string.Empty;
        public ActionType ActionType { get; set; }
        public PortalType Portal { get; set; }
    }
}
