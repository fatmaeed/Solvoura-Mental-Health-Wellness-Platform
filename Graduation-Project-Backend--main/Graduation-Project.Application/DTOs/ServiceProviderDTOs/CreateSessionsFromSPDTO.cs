using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.ServiceProviderDTOs
{
    public class CreateSessionsFromSPDTO
    {
        public int ServiceProviderId { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        [Required]
        public int DurationInMinutes { get; set; }
        public TimeSpan Duration  => TimeSpan.FromMinutes(DurationInMinutes);
        public decimal SessionPrice { get; set; }
        [Required]
        public string Type { get; set; } 
        public int RepeatedFor { get; set; } 
        public DateOnly? ToDate { get; set; }
    }
}
