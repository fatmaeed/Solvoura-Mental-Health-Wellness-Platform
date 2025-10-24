using Graduation_Project.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {

    public class PaymentEntity : BaseEntity {

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }
        public DateTime PaymentDate { get; set; }

        [MaxLength(100)]
        public string PaymentMethod { get; set; }

        public string TransactionId { get; set; }

        public string? Message { get; set; }

        public virtual ReservationEntity Reservation { get; set; }
    }
}