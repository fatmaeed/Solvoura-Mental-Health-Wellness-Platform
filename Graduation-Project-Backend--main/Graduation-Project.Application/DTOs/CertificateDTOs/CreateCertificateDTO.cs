using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Application.DTOs.CertificateDTOs {

    public class CreateCertificateDTO {
        public required IFormFile Image { get; set; }

        [MaxLength(100)]
        public required string CertificateName { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public DateTime IssueDate { get; set; }
    }
}