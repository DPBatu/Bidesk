using Bidesk.Domain.Entities.Public;

namespace Bidesk.Application.ViewModels.ProductionLine
{
    public class ProductionLineVM
    {
        public long Value { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        
        public ProductionLineVM GetValueFromEntity(ProductionLineEnt ent)
        {

            Value = ent.ID;
            Code = ent.Code;
            Name = ent.Name;
            return this;
        }
        public ProductionLineEnt ToEntity()
        {
            return new ProductionLineEnt
            {
                ID = Value,
                Code = Code,
                Name = Name
            };
        }
    }
}
