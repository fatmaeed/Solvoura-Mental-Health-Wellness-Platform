using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {

    public class NotificationEntity : BaseEntity {

        [MaxLength(50)]
        public string Title { get; set; }

        public bool Readed { get; set; } = false;

        [MaxLength(200)]
        public string Message { get; set; }

        [MaxLength(50)]
        public string Type { get; set; }

        [MaxLength(500)]
        public string? Routing { get; set; }

        [ForeignKey("User")]
        public int? SenderId { get; set; }

        public virtual UserEntity? User { get; set; }

        public virtual ICollection<UserNotificationEntity>? Notifications { get; set; }
    }
}