using Graduation_Project.Application.Interfaces.IRepositories;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Infrastructure.Data;

namespace Graduation_Project.Infrastructure.Repositories {

    public class UserNotificationRepo : BaseRepo<UserNotificationEntity>, IUserNotificationRepo {

        public UserNotificationRepo(MentalDbContext mentalDbContext) : base(mentalDbContext) {

        }

        public List<UserNotificationEntity> GetByUserId(int userId) {
            return mentalDbContext.UserNotifications.Where(x => x.UserId == userId).ToList();
        }
    }
}