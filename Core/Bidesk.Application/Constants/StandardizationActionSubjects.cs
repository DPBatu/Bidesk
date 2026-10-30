using System;
using System.Collections.Generic;
using System.Linq;

namespace Bidesk.Application.Constants
{
    /// <summary>
    /// "Standartlastirma ve Surekliligi Saglama" kutucuklarindan dogan aksiyonlarin konu metinleri.
    /// PortalClient tarafindaki `kaizen/constants/standardization-actions.ts` dosyasinin aynasi —
    /// metinler VERI, degistirilemez.
    ///
    /// Neden gerekli: kaizen kaydedilirken ekran YALNIZ bu kutucuklardan dogan aksiyonlari
    /// gonderiyor. Iptal dongusu, listede olmayan her bekleyen aksiyonu iptal ederse merkezi
    /// aksiyon panelinden (action-panel) acilmis aksiyonlar da sessizce yok oluyor. Bu yuzden
    /// iptal YALNIZ standartlastirmadan dogan aksiyonlara uygulanir.
    /// </summary>
    public static class StandardizationActionSubjects
    {
        /// <summary>Yeni kayitlara yazilan isaret; eski satirlarda null.</summary>
        public const string SourceRole = "standardization";

        private static readonly string[] Exact =
        {
            "Standart setup talimatı",
            "Otonom bakım talimatı",
            "Standart iş talimatı",
            "Kontrol planı",
            "Tornalama akışı",
            "FMEA",
            "Standart bakım talimatı",
            "Tek nokta eğitimi",
            "Yaygınlaştırma",
        };

        /// <summary>Serbest metin eklenen iki konu: "Öğrenilmiş ders ID : 12" gibi.</summary>
        private static readonly string[] Prefixes =
        {
            "Öğrenilmiş ders ID :",
            "Diğer :",
        };

        /// <summary>
        /// Aksiyon standartlastirma kutucugundan mi dogmus? Yeni kayitlarda SourceRole yeter;
        /// goc edilmis/eski kayitlarda konu metnine bakilir.
        /// </summary>
        public static bool IsStandardizationAction(string? sourceRole, string? description)
        {
            if (string.Equals(sourceRole, SourceRole, StringComparison.Ordinal))
                return true;

            if (!string.IsNullOrWhiteSpace(sourceRole))
                return false;

            var subject = (description ?? string.Empty).Trim();
            if (subject.Length == 0)
                return false;

            return Exact.Contains(subject)
                || Prefixes.Any(p => subject.StartsWith(p, StringComparison.Ordinal));
        }
    }
}
