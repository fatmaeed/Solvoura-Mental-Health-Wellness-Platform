using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {
    public class SessionMeetingClientsEntity : BaseEntity {
        [ForeignKey("SessionMeetingInfo")]
        public int SessionMeetingId { get; set; }
        public virtual SessionMeetingInfoEntity? SessionMeetingInfo { get; set; }
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public virtual ClientEntity? Client { get; set; }
    }
}
