namespace Bidesk.Application.ViewModels.User
{
    public class DetailedUserInfoVm
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? RegistrationNo { get; set; }
        public string? CardNo { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public string? IntercomNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CompanyPhoneNumber { get; set; }
    }
}
