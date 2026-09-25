using Bidesk.Domain.Entities;

namespace Bidesk.Domain.Entities.Public
{
    public class BusinessPartnerEnt: BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCustomer { get; set; }
    }
}
