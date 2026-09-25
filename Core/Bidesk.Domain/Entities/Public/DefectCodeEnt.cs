using Bidesk.Domain.Entities;

namespace Bidesk.Domain.Entities.Public
{
    public class DefectCodeEnt: BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsTool { get; set; }
        public bool IsSupplier { get; set; }
    }
}
