using Graduation_Project.Domain.Interfaces.IEntities;
using Microsoft.AspNetCore.Identity;

namespace Graduation_Project.Domain.Entities {
    public class UserEntity : IdentityUser<int>, IBaseEntity {
        public virtual ICollection<UserNotificationEntity>? Notifications { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }
        public virtual UserLikes? UserLikes { get; set; }
    }
}