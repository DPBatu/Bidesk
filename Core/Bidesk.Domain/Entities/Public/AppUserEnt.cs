using Microsoft.AspNetCore.Identity;

namespace Bidesk.Domain.Entities.Public
{
    public class AppUserEnt: IdentityUser<long>
    {
        public string RegistrationNo { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string NameSurname { get; set; } = string.Empty;
        public string? CompanyPhoneNumber { get; set; }
        public string? IntercomNumber { get; set; }
        public string? SAPUserName { get; set; }
        public string? ComputerName { get; set; }
        public string EmployeeClassification { get; set; } = string.Empty;
        public DateTime? HireDate { get; set; }
        public DateTime? DismissalDate { get; set; }
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? CardNumber { get; set; }
        public long? DepartmentID { get; set; }
        public DepartmentEnt? Department { get; set; }
        public long? PositionID { get; set; }
        public PositionEnt? Position { get; set; }
        public ICollection<DepartmentEnt> ResponsibleDepartment { get; set; } = new List<DepartmentEnt>();
        public ICollection<UserDepartmentEnt> DepartmentLogs { get; set; } = new List<UserDepartmentEnt>();
        public ICollection<UserPositionEnt> PositionLogs { get; set; } = new List<UserPositionEnt>();
        public bool IsAdmin { get; set; } = false;
        public bool IsJobUser { get; set; } = false;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }
        public bool? IsDismissed { get; set; }
        public bool IsPasswordResetRequired { get; set; } = true;
        public string? ConfirmationCode { get; set; }
        public DateTime? ConfirmationCodeExpiredEndDate { get; set; }
    }
}