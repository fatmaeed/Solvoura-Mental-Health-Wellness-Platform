using Graduation_Project.Application.DTOs.NotificationDTOs;
using Graduation_Project.Domain.Entities;

namespace Graduation_Project.Application.Interfaces.IServices {
    public interface INotificationService {
        public Task SendNotification(int userId, CreateNotificationDTO notification);
        public Task SendNotificationToAll(CreateNotificationDTO notification);

      
    }
}
