using Graduation_Project.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {

    public class ReservationEntity : BaseEntity {
        public DateTime Date { get; set; }
        public int SessionsNumber { get; set; } = 0;
        public int RamainingSessionsNumber { get; set; } = 0;
        public int DoneSessionsNumber { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public ReservationType ReservationType { get; set; }

        [ForeignKey("ServiceProvider")]
        public int ServiceProviderId { get; set; }

        public virtual ServiceProviderEntity ServiceProvider { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        public virtual ClientEntity Client { get; set; }

        [ForeignKey("Payment")]
        public int PaymentId { get; set; }

        public virtual PaymentEntity Payment { get; set; }
        public virtual List<SessionEntity> Sessions { get; set; }
    }
}