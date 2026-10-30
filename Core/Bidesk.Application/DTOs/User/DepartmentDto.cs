using Bidesk.Application.ViewModels.Primeng;
using Bidesk.Domain.Entities.Public;

namespace Bidesk.Application.DTOs.User
{
    public class DepartmentDto
    {
        public long Id { get; set; }
        public string Description { get; set; } = string.Empty;

        public DepartmentDto FromEntity(DepartmentEnt entity)
        {
            Id = entity.ID;
            Description = entity.Description;
            return this;
        }

        public DepartmentEnt ToEntity()
        {
            return new DepartmentEnt
            {
                ID = Id,
                Description = Description
            };
        }

        public DropDownVm ToDropDown()
        {
            return new DropDownVm
            {
                Key = Id,
                Label = Description
            };
        }
    }
}
