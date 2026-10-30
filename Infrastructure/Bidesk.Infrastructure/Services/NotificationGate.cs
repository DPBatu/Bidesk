using Microsoft.Extensions.Configuration;

namespace Bidesk.Infrastructure.Services
{
    /// <summary>
    /// Giden bildirimler (mail / SMS / WhatsApp) icin global acma-kapama kapisi.
    /// appsettings.json -> "Notifications" bolumu ile yonetilir.
    /// Anahtar tanimli degilse varsayilan davranis ACIK'tir (geriye donuk uyumluluk).
    /// </summary>
    public static class NotificationGate
    {
        private const string SectionName = "Notifications";

        /// <summary>Ana salter. Kapaliysa hicbir kanaldan gonderim yapilmaz.</summary>
        public static bool IsEnabled(IConfiguration configuration)
            => ReadBool(configuration, "Enabled");

        /// <summary>Ana salter VE ilgili kanal ("Mail", "Sms", "WhatsApp") acik mi?</summary>
        public static bool IsChannelEnabled(IConfiguration configuration, string channel)
            => IsEnabled(configuration) && ReadBool(configuration, channel);

        /// <summary>
        /// "Notifications:TestRecipient" doluysa tum alicilar bu adrese yonlendirilir.
        /// Bos ise gercek alicilar aynen dondurulur.
        /// </summary>
        public static string[] ResolveRecipients(IConfiguration configuration, string[] tos)
        {
            var testRecipient = configuration[$"{SectionName}:TestRecipient"];

            if (!string.IsNullOrWhiteSpace(testRecipient))
                return new[] { testRecipient.Trim() };

            return tos ?? Array.Empty<string>();
        }

        private static bool ReadBool(IConfiguration configuration, string key)
        {
            var value = configuration[$"{SectionName}:{key}"];

            if (string.IsNullOrWhiteSpace(value))
                return true;

            return !bool.TryParse(value, out var parsed) || parsed;
        }
    }
}
