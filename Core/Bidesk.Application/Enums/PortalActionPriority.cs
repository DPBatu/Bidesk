namespace Bidesk.Application.Enums
{
    /// <summary>
    /// PortalAPI'nin merkezi aksiyon onceligi (`Portal.Application.Enums.Action.ActionPriorityEnum`)
    /// kopyasi. Depolar arasinda kod paylasimi olmadigi icin kopyalanir; DEGERLER
    /// Portal'daki ile birebir ayni kalmalidir.
    ///
    /// <para>Kaizen ekraninda oncelik alani YOK; toplu kaydetmede acilan aksiyonlar
    /// <see cref="Normal"/> ile dogar, kullanici isterse merkezi ekrandan degistirir.</para>
    /// </summary>
    public enum PortalActionPriority : byte
    {
        Low = 1,
        Normal = 2,
        High = 3,
        Urgent = 4
    }
}
