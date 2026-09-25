using Bidesk.Domain.Entities.Public;

namespace Bidesk.Application.DTOs.ProductionLine
{
    public class ProductionLineDto
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public ProductionLineDto GetValueEntity(ProductionLineEnt ent)
        {
            Code = ent.Code;
            Name = ent.Name;
            return this;
        }
    }
}
