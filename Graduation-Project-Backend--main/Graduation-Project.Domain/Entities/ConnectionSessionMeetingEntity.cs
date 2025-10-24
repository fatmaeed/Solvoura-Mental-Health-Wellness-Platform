using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {
    public class ConnectionSessionMeetingEntity : BaseEntity {
        public required string ConnectionCreatorId { get; set; }
        public required string IceCandidate { get; set; }
        public required string Offer { get; set; }
        public required string Answer { get; set; }
        [ForeignKey("Session")]
        public int SessionId { get; set; }

        public virtual SessionEntity? Session { get; set; }

    }
}
