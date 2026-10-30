using Bidesk.Domain.Entities;

namespace Bidesk.Domain.Entities.Public
{
    public class ProductionOperationEnt: BaseEntity
    {
        public string? Code { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
