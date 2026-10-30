using Bidesk.Domain.Entities.Public;

namespace Bidesk.Application.DTOs.User
{
    public class NotificationUserDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public bool IsRead { get; set; } = false;

        public NotificationUserDto FromEntity(NotificationUserEnt entity)
        {
            Id = entity.ID;
            UserId = entity.UserId;
            IsRead = entity.IsRead;
            return this;
        }

        public NotificationUserEnt ToEntity()
        {
            return new NotificationUserEnt
            {
                ID = Id,
                UserId = UserId,
                IsRead = IsRead
            };
        }
    }
}