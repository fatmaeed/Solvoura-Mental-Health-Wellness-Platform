using Graduation_Project.Application.Common.Attributes;
using Graduation_Project.Application.DTOs.CertificateDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.ServiceProviderDTOs
{
    public class EditServiceProviderDto
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string FirstName { get; set; }

        [MaxLength(20)]
        public string LastName { get; set; }

        public IFormFile? UserImage { get; set; }

        [MaxLength(1500)]
        public string? ClinicLocation { get; set; }

        [MaxLength(500), MinLength(10)]
        public string Description { get; set; }

        [Range(0, 3)]
        public int ExaminationType { get; set; }


        [Range(0.0, 10000.0)]
        public decimal PricePerHour { get; set; }
        public int NumberOfExp { get; set; } = 0;
        public string? Experience { get; set; }
        public List<AddCertificateDTO>? Certificates { get; set; }
    }
    public class AddCertificateDTO
    {
        public int ServiceProviderId { get; set; }
        public required IFormFile Image { get; set; }

        [MaxLength(100)]
        public required string CertificateName { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public DateTime IssueDate { get; set; }
    }
}
