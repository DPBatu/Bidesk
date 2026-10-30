using Bidesk.Domain.Entities;

namespace Bidesk.Domain.Entities.Public
{
    public class RoleRelationEnt: BaseEntity
    {
        public long ParentID { get; set; }
        public AppRoleEnt Parent { get; set; } = null!;
        public long ChildId { get; set; }
        public AppRoleEnt Child { get; set; } = null!;
    }
}
