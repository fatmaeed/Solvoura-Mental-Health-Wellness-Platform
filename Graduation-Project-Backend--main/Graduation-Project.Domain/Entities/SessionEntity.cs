using Graduation_Project.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {

    public class SessionEntity : BaseEntity {
        public DateTime StartDateTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime? EndDateTime { get; set; }
        public SessionStatus Status { get; set; } = 0;

        [Column(TypeName = "money")]
        public decimal SessionPrice { get; set; }

        public SessionType Type { get; set; }
        public virtual PrescriptionEntity? Prescription { get; set; }
        public virtual List<FeedBackEntity>? FeedBacks { get; set; }

        [ForeignKey("ServiceProvider")]
        public int? ServiceProviderId { get; set; }

        public virtual ServiceProviderEntity ServiceProvider { get; set; }

        [ForeignKey("Reservation")]
        public int? ReservationId { get; set; }

        public virtual ReservationEntity? Reservation { get; set; }
        public bool Reminded { get; set; } = false;
    }
}