using Bidesk.Domain.Entities.Public;

namespace Bidesk.Domain.Entities
{
    public class BaseEntity
    {
        public long ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedUserId { get; set; }
        public AppUserEnt CreatedUser { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedUserId { get; set; }
        public AppUserEnt? UpdatedUser { get; set; }
        public bool IsCanceled { get; set; } = false;
        public DateTime? CanceledDate { get; set; }
        public long? CanceledUserId { get; set; }
        public AppUserEnt? CanceledUser { get; set; }
        public long CompanyID { get; set; }
        public CompanyEnt? Company { get; set; }
    }
}
