using Bidesk.Domain.Entities;

namespace Bidesk.Domain.Entities.Public
{
    /// <summary>
    /// PortalAPI'nin sahibi olduğu merkezî aksiyon tablosunun (<c>public.Actions</c>)
    /// SALT-OKUNUR aynası.
    ///
    /// <para>Aksiyon Kaizen ekranından doğar ama yaşam döngüsü (kabul/ret, tamamlama,
    /// onay/ret) PortalAPI'de işler; yazma her zaman <c>api/portal/Action</c> uçlarından
    /// geçer. Bu kopya yalnızca OKUMAK için var: kaizen kaydı görüntülendiğinde
    /// aksiyonlara bakıp master statüyü güncel tutabilmek için.</para>
    ///
    /// <para>Migration dışında tutulur (<c>ExcludeFromMigrations</c>) — tablonun şeması
    /// Portal'ın migration'larıyla yönetilir. Alan eklendiğinde bu kopya da güncellenir;
    /// aksi halde EF bilinmeyen kolonu sorguya katmaz, hata vermez, sessizce eksik okur.</para>
    /// </summary>
    public class ActionEnt : BaseEntity
    {
        public string ModuleKey { get; set; } = string.Empty;

        /// <summary>Şema önekli fiziksel tablo adı — kaizen aksiyonlarında <c>Kaizen.Kaizens</c>.</summary>
        public string TableName { get; set; } = string.Empty;

        /// <summary>Master kaydın ID'si — kaizen aksiyonlarında <c>KaizenID</c>.</summary>
        public long TableID { get; set; }

        /// <summary>Aynı master kayıtta birden çok aksiyon rolü varsa hangisi; Kaizen'de <c>null</c>.</summary>
        public string? SourceRole { get; set; }

        /// <summary>Kaynağın alt kırılımı (Öneri'de değerlendirme ID'si); Kaizen'de <c>null</c>.</summary>
        public long? SourceSubID { get; set; }

        /// <summary>Aksiyonu açan (A). <c>CreatedUserId</c> denetim alanıdır, atayan budur.</summary>
        public long AssignerUserID { get; set; }
        public AppUserEnt? AssignerUser { get; set; }

        public string Description { get; set; } = string.Empty;
        public string? Cause { get; set; }
        public byte Priority { get; set; }
        public DateTime DueDate { get; set; }

        /// <summary>Aksiyonun sorumlusu — merkezi tabloda ZORUNLU.</summary>
        public long ResponsibleUserID { get; set; }
        public AppUserEnt? ResponsibleUser { get; set; }


        /// <summary>Portal'daki <c>ActionStatusEnum</c>: 1 Bekliyor … 6 Atayan Ret.</summary>
        public byte Status { get; set; }

        public DateTime? PlanDate { get; set; }
        public DateTime? DoDate { get; set; }
        public DateTime? CheckDate { get; set; }
        public DateTime? ActDate { get; set; }

        public DateTime? AcceptanceDate { get; set; }
        public string? AcceptanceComment { get; set; }

        public DateTime? CompletionDate { get; set; }
        public string? CompletionComment { get; set; }

        public DateTime? DecisionDate { get; set; }
        public string? DecisionComment { get; set; }

        public long? ViaRejectID { get; set; }

        /// <summary>
        /// Aksiyonun kaçıncı kez yeniden gönderildiği. Atayan işi yetersiz bulup
        /// "Yeniden Gönder" dedikçe zincir boyunca birer artar; Portal yazar.
        /// </summary>
        public byte ReopenCount { get; set; }

        /// <summary>
        /// Modül yöneticilerine hangi <see cref="ReopenCount"/> değerinde bilgi maili
        /// gönderildiği. Bu kolonu MODÜL yazar: yönetici listesi modül şemasında ve
        /// Portal onu bilmez.
        /// </summary>
        public byte ReopenAlertedCount { get; set; }

        public DateTime? LastReminderDate { get; set; }
        public byte ReminderCount { get; set; }
    }
}
