using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {

    public class PrescriptionEntity : BaseEntity {
        public string Description { get; set; }

        public string? ImagePath { get; set; }

        [ForeignKey("Session")]
        public int SessionId { get; set; }

        public virtual SessionEntity Session { get; set; }
    }
}