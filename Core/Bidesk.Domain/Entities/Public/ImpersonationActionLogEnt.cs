namespace Bidesk.Domain.Entities.Public
{
    /// <summary>
    /// Portal'ın taklit (impersonation) işlem kaydı — public şemadan OKUNUR/YAZILIR,
    /// tablonun sahibi Portal'dır (bu bağlamda migration üretilmez).
    ///
    /// Taklit oturumunun token'ı bütün portallarda geçerli olduğundan, Kaizen'de yapılan
    /// değiştiren istekler de aynı merkezi tabloya düşer; aksi halde "bu kaydı aslında hangi
    /// yönetici değiştirdi" bilgisi Kaizen tarafında kaybolurdu.
    /// </summary>
    public class ImpersonationActionLogEnt : BaseEntity
    {
        public long SessionID { get; set; }
        public long AdminUserID { get; set; }
        public long TargetUserID { get; set; }
        public string HttpMethod { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string? Source { get; set; }
        public int StatusCode { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}
