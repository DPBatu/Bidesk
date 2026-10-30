using Microsoft.AspNetCore.Identity;

namespace Bidesk.Domain.Entities.Public
{
    public class AppRoleEnt: IdentityRole<long>
    {
        public ICollection<EndpointEnt> Endpoints { get; set; } = new List<EndpointEnt>();
        public ICollection<RouteEnt> Routes { get; set; } = new List<RouteEnt>();
    }
}
