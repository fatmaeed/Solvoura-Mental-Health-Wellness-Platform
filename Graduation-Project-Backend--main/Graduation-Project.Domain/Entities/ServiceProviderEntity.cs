using Graduation_Project.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {
    [Index(nameof(NationalId), IsUnique = true)]
    public class ServiceProviderEntity : PersonEntity {

        [MaxLength(14), RegularExpression("^[0-9]{14}$")]
        public required string NationalId { get; set; }

        public Specialization Specialization { get; set; }

        public required string UserImagePath { get; set; }

        public required string NationalImagePath { get; set; }

        public required string UserAndNationalImagePath { get; set; }

        public string? ClinicLocation { get; set; }
        public required string Description { get; set; }

        public ReservationType ExaminationType { get; set; }
        public int NumberOfExp { get; set; } = 0;
        public string? Experience { get; set; }

        [Column(TypeName = "money")]
        public decimal PricePerHour { get; set; }

        public bool IsAprroved { get; set; } = false;
        public virtual required List<CertificateEntity> Certificates { get; set; }
    }
}