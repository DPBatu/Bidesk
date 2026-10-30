using Bidesk.Domain.Entities.Public;

namespace Bidesk.Application.DTOs.User
{
    public class UserDto
    {
        public long ID { get; set; }
        public string RegistrationNo { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string NameSurname { get; set; }
        public string PhoneNumber { get; set; }
        public string? CompanyPhoneNumber { get; set; }
        public string? IntercomNumber { get; set; }
        public string? SAPUserName { get; set; }
        public string? ComputerName { get; set; }
        public string EmployeeClassification { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? DismissalDate { get; set; }
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? CardNumber { get; set; }
        public DepartmentEnt? Department { get; set; }
        public PositionEnt? Position { get; set; }
        public ICollection<DepartmentEnt> ResponsibleDepartment { get; set; } = new List<DepartmentEnt>();
        public ICollection<UserDepartmentEnt> DepartmentLogs { get; set; } = new List<UserDepartmentEnt>();
        public ICollection<UserPositionEnt> PositionLogs { get; set; } = new List<UserPositionEnt>();
        public bool IsAdmin { get; set; } = false;
    }
}
