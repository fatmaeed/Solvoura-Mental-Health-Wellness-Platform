using AutoMapper;
using Graduation_Project.Application.DTOs.NotificationDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Domain.Entities;

namespace Graduation_Project.Application.Services {

    public class NotificationService : INotifiService {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task Add(CreateNotificationDTO notification) {
            if (notification == null) {
                throw new ArgumentNullException(nameof(notification));
            }

            var noti = _mapper.Map<NotificationEntity>(notification);
            _unitOfWork.NotificationRepo.Add(noti);
            _unitOfWork.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public Task Delete(int id) {
            var noti = _unitOfWork.NotificationRepo.Get(id);
            if (noti == null) {
                throw new ArgumentNullException(nameof(noti));
            }
            _unitOfWork.NotificationRepo.Delete(id);
            _unitOfWork.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public IEnumerable<DisplayNotificationDTO> GetAllNotificationsForUser(int userId) {
            List<UserNotificationEntity> userNotifications = _unitOfWork.UserNotificationRepo.GetByUserId(userId);

            if (userNotifications == null) {
                throw new ArgumentNullException(nameof(userNotifications));
            }
            var notification = _mapper.Map<List<DisplayNotificationDTO>>(userNotifications);
            return notification;
        }

        public IEnumerable<DisplayNotificationDTO> GetAllUnread(int userId) {
            List<UserNotificationEntity> userNotifications = _unitOfWork.UserNotificationRepo.GetByUserId(userId);

            if (userNotifications == null) {
                throw new ArgumentNullException(nameof(userNotifications));
            }
            var notification = _mapper.Map<List<DisplayNotificationDTO>>(userNotifications.Where(un => un.Notification!.Readed == false).ToList());
            return notification;
        }

        public DisplayNotificationDTO? GetNotification(int id) {
            var noti = _unitOfWork.NotificationRepo.Get(id);
            if (noti == null) {
                throw new ArgumentNullException(nameof(noti));
            }
            var notification = _mapper.Map<DisplayNotificationDTO>(noti);
            return notification;
        }

        public async Task Update(NotificationUpdateDto notification) {
            try {
                var noti = _mapper.Map<NotificationEntity>(notification);
                _unitOfWork.NotificationRepo.Update(noti);
                await _unitOfWork.SaveChangesAsync();
            } catch (Exception ex) {
                throw new Exception($"Error updating Noti: {ex.Message}");
            }
        }
    }
}