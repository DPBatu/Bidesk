using Bidesk.Domain.Entities;

namespace Bidesk.Domain.Entities.Public
{
    public class MenuEnt: BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<EndpointEnt> Endpoints { get; set; } = new List<EndpointEnt>();
    }
}
