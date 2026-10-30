namespace Bidesk.Application.DTOs.User
{
    /// <summary>
    /// Personel listesi icin hafif kullanici projeksiyonu (departman adi dahil).
    /// GetAllActiveUsersAsync departmani Include etmedigi icin ayri bir sorgu kullanilir.
    /// </summary>
    public class PersonnelDto
    {
        public long ID { get; set; }
        public string? NameSurname { get; set; }
        public string? RegistrationNo { get; set; }
        public string? DepartmentName { get; set; }
    }
}
