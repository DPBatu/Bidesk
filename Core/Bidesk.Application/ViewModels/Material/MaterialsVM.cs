using Bidesk.Domain.Entities.Public;

namespace Bidesk.Application.ViewModels.Material
{
    public class MaterialsVM
    {
        public long Value { get; set; }
        public  string Code { get; set; }
        public string Name { get; set; }

        public MaterialsVM GetValueFromEntity(MaterialEnt ent)
        {
            if (ent != null)
            {
                Value = ent.ID;
                Code = ent.Code;
                Name = ent.Name;
                return this;
            }
            else return null;
            
            

        }
        public MaterialEnt ToEntity()
        {
            return new MaterialEnt
            {
                ID = Value,
                Code = Code,
                Name = Name
            };
        }   
    }
}
