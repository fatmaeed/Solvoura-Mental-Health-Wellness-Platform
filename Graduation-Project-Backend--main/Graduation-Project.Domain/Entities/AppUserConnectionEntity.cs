using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {

    public class AppUserConnectionEntity : BaseEntity {
        public string ConnectionId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual UserEntity? User { get; set; }
    }
}