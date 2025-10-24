using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {

    public class SessionMeetingInfoEntity : BaseEntity {

        [ForeignKey("Session")]
        public int SessionId { get; set; }

        public virtual SessionEntity? Session { get; set; }

        [Range(1, 180)]
        public int? DurationInMins { get; set; } = 0;

        public DateTime? StartSessionTime { get; set; }

        public DateTime? EndSessionTime { get; set; }

        public bool IsCompleted { get; set; } = false;

        public virtual ServiceProviderEntity? ServiceProvider { get; set; }

        [ForeignKey("ServiceProvider")]
        public int ServiceProviderId { get; set; }

        public virtual ICollection<ClientEntity>? Clients { get; set; }
    }
}