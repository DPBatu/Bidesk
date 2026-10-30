namespace Bidesk.Application.Enums
{
    /// <summary>
    /// PortalAPI'nin merkezi aksiyon statusu (`Portal.Application.Enums.Action.ActionStatusEnum`)
    /// kopyasi. Depolar arasinda kod paylasimi olmadigi icin — BaseEntity, AppUserEnt ve
    /// diger public varliklar gibi — bu enum da kopyalanir.
    ///
    /// <para>DEGERLER Portal'daki ile birebir ayni kalmalidir; degistigi anda burasi da
    /// guncellenir. Kaizen'in KENDI KaizenActionStatusEnum'u ile KARISTIRILMAMALIDIR:
    /// oradaki degerler (Waiting=1, Completed=2, Approved=3, Rejected=4) farklidir.</para>
    /// </summary>
    public enum PortalActionStatus : byte
    {
        Waiting = 1,
        AcceptedByOwner = 2,
        RejectedByOwner = 3,
        Completed = 4,
        ApprovedByAssigner = 5,
        RejectedByAssigner = 6
    }
}
