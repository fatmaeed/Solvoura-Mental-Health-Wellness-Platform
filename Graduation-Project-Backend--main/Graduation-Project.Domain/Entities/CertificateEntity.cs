using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {

    public class CertificateEntity : BaseEntity {
        public string ImagePath { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }
        public DateTime IssueDate { get; set; }

        [ForeignKey("ServiceProvider")]
        public int ServiceProviderId { get; set; }

        public virtual ServiceProviderEntity ServiceProvider { get; set; }
    }
}