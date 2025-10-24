using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {

    public class UserNotificationEntity : BaseEntity {

        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual UserEntity? User { get; set; }

        [ForeignKey("Notification")]
        public int NotificationId { get; set; }

        public virtual NotificationEntity? Notification { get; set; }
    }
}