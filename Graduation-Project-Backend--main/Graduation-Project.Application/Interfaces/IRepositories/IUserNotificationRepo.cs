using Graduation_Project.Domain.Entities;

namespace Graduation_Project.Application.Interfaces.IRepositories {

    public interface IUserNotificationRepo : IBaseRepo<UserNotificationEntity> {
        List<UserNotificationEntity> GetByUserId(int userId);
    }
}