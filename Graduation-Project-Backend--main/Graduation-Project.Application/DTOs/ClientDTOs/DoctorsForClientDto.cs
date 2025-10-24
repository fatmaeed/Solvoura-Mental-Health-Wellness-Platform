using Graduation_Project.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.ClientDTOs
{
    public class DoctorsForClientDto
    {
        public string Name { get; set; }
        public Specialization Specialization { get; set; }
        public int SessionsCount { get; set; }
        public  string UserImagePath { get; set; }

        public string? ClinicLocation { get; set; }
        public  string Description { get; set; }

        public ReservationType ExaminationType { get; set; }
        public int NumberOfExp { get; set; } = 0;
        public string? Experience { get; set; }

        [Column(TypeName = "money")]
        public decimal PricePerHour { get; set; }
    }
}
