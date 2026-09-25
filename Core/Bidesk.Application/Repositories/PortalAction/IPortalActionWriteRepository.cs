using Bidesk.Domain.Entities.Public;

namespace Bidesk.Application.Repositories.PortalAction
{
    /// <summary>
    /// Merkezi aksiyon tablosuna (public.Actions) YAZMA erisimi.
    ///
    /// <para><b>Sinir:</b> Buradan yalnizca aksiyonun TANIM alanlari yazilir — kaizen
    /// kaydedilirken toplu olusturulan/guncellenen aksiyonlar ve personel devri.
    /// Aksiyonun YASAM DONGUSU (kabul/ret, tamamlama, onay/ret) PortalAPI'nin
    /// ActionService'inde kalir; Status ve PDCA alanlari BURADAN ASLA yazilmaz.</para>
    ///
    /// <para>Neden dogrudan yaziliyor: kaizen ekrani aksiyonlari tek tek degil, master
    /// kayitla BIRLIKTE kaydediyor. Ayni transaction'da kalabilmek icin yazma ayni
    /// DbContext uzerinden yapiliyor; servisler arasi HTTP cagrisi yok.</para>
    /// </summary>
    public interface IPortalActionWriteRepository : IWriteRepository<ActionEnt>
    {
    }
}
