using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.ServiceProviderDTOs
{
    public class UpdateServiceProviderDTO
    {
        [Required]

        public string? Specialization { get; set; }

        public bool? IsApproved { get; set; }

        public string? Description { get; set; }

        public int? NumberOfExp { get; set; }

        public string? Experience { get; set; }

        public decimal? PricePerHour { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? Address { get; set; }

        public string? ClinicLocation { get; set; }

        public string? UserImagePath { get; set; }

        public string? NationalImagePath { get; set; }

        public string? UserAndNationalImagePath { get; set; }

        
    }
}

