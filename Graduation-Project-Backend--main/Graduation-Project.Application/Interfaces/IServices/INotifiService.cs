using Graduation_Project.Application.DTOs.NotificationDTOs;

namespace Graduation_Project.Application.Interfaces.IServices {
    public interface INotifiService {
        public DisplayNotificationDTO? GetNotification(int id);
        public IEnumerable<DisplayNotificationDTO> GetAllUnread(int userId);
        public IEnumerable<DisplayNotificationDTO> GetAllNotificationsForUser(int userId);
        Task Add(CreateNotificationDTO notification);
        Task Update(NotificationUpdateDto notification);
        Task Delete(int id);
    }
}
