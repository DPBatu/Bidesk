namespace Bidesk.Domain.Entities.Public
{
    public class UserPositionEnt: BaseEntity
    {
        public long UserID { get; set; }
        public AppUserEnt? User { get; set; }
        public long PositionID { get; set; }
        public PositionEnt? Position { get; set; }
    }
}
