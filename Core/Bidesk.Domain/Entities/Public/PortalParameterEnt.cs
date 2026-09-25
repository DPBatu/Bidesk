namespace Bidesk.Domain.Entities.Public
{
    /// <summary>
    /// Portal'ın tek satırlık genel ayar kaydının (<c>public.PortalParameters</c>)
    /// SALT-OKUNUR aynası.
    ///
    /// <para>Kaizen'in kendi parametre tablosuyla karıştırılmamalı: bu, tüm portallar için
    /// geçerli olan ve Portal admin ekranından yönetilen ayarlardır. Burada yalnızca
    /// Kaizen'in ihtiyaç duyduğu alan tutuluyor.</para>
    ///
    /// <para>Migration dışında tutulur; şemanın sahibi Portal'dır.</para>
    /// </summary>
    public class PortalParameterEnt : BaseEntity
    {
        /// <summary>
        /// Bir aksiyon kaç kez yeniden gönderilirse modül yöneticilerine bilgi maili
        /// gideceği. 0 kapatır.
        /// </summary>
        public byte ActionReopenAlertThreshold { get; set; }
    }
}
