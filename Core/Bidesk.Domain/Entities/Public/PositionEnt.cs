using Bidesk.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Bidesk.Domain.Entities.Public
{
    public class PositionEnt: BaseEntity
    {
        public long? UpperPositionID { get; set; }
        public string? Code { get; set; }
        public string Description { get; set; } = string.Empty;
        public ICollection<AppUserEnt> Employees { get; set; } = new List<AppUserEnt>();
    }
}
