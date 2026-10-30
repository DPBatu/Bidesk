using Bidesk.Domain.Entities.Public;

namespace Bidesk.Application.Repositories.PortalAction
{
    /// <summary>
    /// Merkezi aksiyon tablosunun (public.Actions) okuma erisimi.
    /// Yazma icin bkz. <see cref="IPortalActionWriteRepository"/> — yalnizca tanim
    /// alanlari ve devir icindir; yasam dongusu PortalAPI'de isler.
    /// </summary>
    public interface IPortalActionReadRepository : IReadRepository<ActionEnt>
    {
    }
}
