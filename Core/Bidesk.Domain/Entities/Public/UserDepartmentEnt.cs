using Bidesk.Domain.Entities;

namespace Bidesk.Domain.Entities.Public
{
    public class UserDepartmentEnt: BaseEntity
    {
        public long UserID { get; set; }
        public AppUserEnt? User { get; set; }
        public long DepartmentID { get; set; }
        public DepartmentEnt? Department { get; set; }
    }
}
