using Graduation_Project.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.ReservationDTOs
{
    public class SessionDto
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public TimeSpan Duration { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public decimal SessionPrice { get; set; }
    }
}
