namespace Bidesk.Domain.Entities.Public
{
    public class RouteEnt: BaseEntity
    {
        public string Route { get; set; } = string.Empty;
        public ICollection<AppRoleEnt> Roles { get; set; } = new List<AppRoleEnt>();
    }
}
