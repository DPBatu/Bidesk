using Bidesk.Domain.Entities;

namespace Bidesk.Domain.Entities.Public
{
    public class CompanyEnt: BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
