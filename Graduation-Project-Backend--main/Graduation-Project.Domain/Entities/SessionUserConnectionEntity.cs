using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {

    public class SessionUserConnectionEntity : BaseEntity {
        public required string ConnectionId { get; set; }

        [ForeignKey("Session")]
        public int? SessionId { get; set; }

        public virtual SessionEntity? Session { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual UserEntity? User { get; set; }
    }
}