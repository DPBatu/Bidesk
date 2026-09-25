namespace Bidesk.Application.Abstraction.Services
{
    public interface IContextService
    {
        public string GetCurrentUserName();
        public long GetCurrentUserId();
        public long GetCurrentCompanyId();
        public bool IsAdmin();
        public string GetCurrentUserEmail();

        /// <summary>
        /// Istek bir kullanici taklidi (impersonation) oturumundan mi geliyor? Oyleyse
        /// GERCEK aktorun (yonetici) kullanici id'si, degilse null. Kimlik bilgisi
        /// (GetCurrentUserId) taklit edilen kullaniciyi gosterir.
        /// </summary>
        public long? GetImpersonatedByUserId();

        /// <summary>Taklit oturumunun kaydi (Portal.ImpersonationSessions.ID); taklit yoksa null.</summary>
        public long? GetImpersonationSessionId();

        /// <summary>Taklit oturumunda yazma serbest mi? Taklit yoksa true (normal oturum).</summary>
        public bool IsWriteAllowed();
    }
}
