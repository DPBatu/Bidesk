using Bidesk.Application.DTOs.User;

namespace Bidesk.Application.Abstraction.Services
{
    public interface INotificationService
    {
        // Bildirimi kaydeder ve SignalR ile ilgili kullanıcı(lar)a anlık gönderir.
        public Task AddAsync(NotificationDto notification);

        /// <summary>
        /// Verilen URL'lere işaret eden tüm bildirimleri (ve kullanıcı satırlarını) iptal eder.
        /// </summary>
        /// <remarks>
        /// Bildirim ile kaynak kayıt arasında FK yoktur; tek bağ NotificationEnt.Url alanıdır.
        /// Kaizen bildirimleri <c>/kaizen/show{bak|stk|a3k}/{KaizenID}</c> kalıbıyla üretildiği için
        /// tam eşleşme (prefix değil) ile hedef kayda ait bildirimler kesin olarak bulunabilir.
        /// Kaizen tipi zamanla değişmiş olabileceğinden birden fazla URL verilebilir.
        ///
        /// Kayıt silindiğinde bildirim durduğu için kullanıcı bildirime tıkladığında boş sayfa açılıyordu.
        /// </remarks>
        /// <param name="urls">Bildirimlerin tam URL değerleri.</param>
        /// <returns>İptal edilen bildirim kullanıcı satırı sayısı.</returns>
        public Task<int> RemoveByUrlsAsync(params string[] urls);
    }
}
