using Bidesk.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bidesk.Domain.Entities.Public
{
    public class DepartmentEnt: BaseEntity
    {
        public long? UpperDepartmentID { get; set; }
        public DepartmentEnt? UpperDepartment { get; set; }
        public string? Code { get; set; }
        public string Description { get; set; } = string.Empty;
        public long? ResponsibleID { get; set; }
        [ForeignKey("ResponsibleID")]
        public AppUserEnt? Responsible { get; set; }
        public ICollection<AppUserEnt> Employees { get; set; } = new List<AppUserEnt>();
    }
}
