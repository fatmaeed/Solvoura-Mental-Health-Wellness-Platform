using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Domain.Enums;

namespace Graduation_Project.Application.DTOs.ServiceProviderDTOs
{
    public class DisplaySessionsForSPDTO
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string Status { get; set; }

        [Column(TypeName = "money")]
        public decimal SessionPrice { get; set; }

        public string Type { get; set; }
        public bool IsReversed { get; set; } 
    }
}
