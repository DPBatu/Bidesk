using Bidesk.Application.ViewModels.User;
using Bidesk.Domain.Entities.Public;

namespace Bidesk.Application.DTOs.User
{
    public class NotificationDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? PortalName { get; set; }
        public List<NotificationUserDto> Users { get; set; } = new List<NotificationUserDto>();
        public DateTime CreatedDate { get; set; }

        public NotificationDto() { }

        public NotificationDto(string title, string content, long[] userIds, string? url = null, string? portalName = null)
        {
            Title = title;
            Content = content;
            Url = url;
            PortalName = portalName;
            Users = userIds.Select(userId => new NotificationUserDto { UserId = userId }).ToList();
        }

        public NotificationDto FromEntity(NotificationEnt entity)
        {
            return new NotificationDto()
            {
                Id = entity.ID,
                Title = entity.Title,
                Content = entity.Content,
                Url = entity.Url,
                PortalName = entity.PortalName,
                Users = entity.Users.Select(x => new NotificationUserDto().FromEntity(x)).ToList(),
                CreatedDate = entity.CreatedDate,
            };
        }

        public NotificationEnt ToEntity()
        {
            return new NotificationEnt
            {
                Title = Title,
                Content = Content,
                Url = Url,
                PortalName = PortalName,
                Users = Users.Select(u => u.ToEntity()).ToList()
            };
        }

        // Bildirim + kullanıcı birleşmiş, hub'a gönderilecek düz liste (NotificationVm).
        public List<NotificationVm> ToFlatList()
        {
            return Users.Select(x => new NotificationVm()
            {
                NotificationId = Id,
                Id = x.Id,
                Title = Title,
                Content = Content,
                Url = Url,
                PortalName = PortalName,
                UserId = x.UserId,
                IsRead = x.IsRead,
                CreatedDate = CreatedDate
            }).ToList();
        }
    }
}
