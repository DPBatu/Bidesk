namespace Bidesk.Application.ViewModels.User
{
    public class UserInfoVm
    {
        public long ID { get; set; }
        public string SicilNo { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string AdiSoyadi { get; set; }
        public string IpAdresi { get; set; }
        public string BilgisayarAdi { get; set; }
        public long DepartmanId { get; set; }
        public string DepartmanTanimi { get; set; }
        public long PozisyonId { get; set; }
        public int PozisyonTanimi { get; set; }
        public string Statu { get; set; }
        public bool FirstLogin { get; set; }
        public bool IsPasswordResetRequired { get; set; }
        public string? ConfirmationCode { get; set; }
        public DateTime? ConfirmationCodeExpiredEndDate { get; set; }
    }
}
