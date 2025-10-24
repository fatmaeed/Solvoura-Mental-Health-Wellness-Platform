using Graduation_Project.Application.DTOs.CertificateDTOs;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Application.DTOs.AccountDTOs {

    public class RegisterServiceProviderDTO : PersonDTO {

        [MaxLength(14), RegularExpression("^[0-9]{14}$"), MinLength(14)]
        public required string NationalNumber { get; set; }

        [Range(0, 6)]
        public int Specialization { get; set; }

        public required IFormFile UserImage { get; set; }

        public required IFormFile NationalImage { get; set; }

        public required IFormFile UserAndNationalImage { get; set; }

        [MaxLength(1500)]
        public string? ClinicLocation { get; set; }

        [MaxLength(500), MinLength(10)]
        public required string Description { get; set; }

        [Range(0, 3)]
        public int ExaminationType { get; set; }

        [Range(0, 60)]
        public int ExperienceInYears { get; set; } = 0;

        [MaxLength(500)]
        public string? ExperienceDescription { get; set; }

        [Range(0.0, 10000.0)]
        public decimal PricePerHour { get; set; }

        public required List<CreateCertificateDTO> Certificates { get; set; }
    }
}