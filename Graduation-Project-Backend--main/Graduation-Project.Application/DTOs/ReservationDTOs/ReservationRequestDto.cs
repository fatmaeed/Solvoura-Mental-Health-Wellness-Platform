using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.ReservationDTOs
{
    public class ReservationRequestDto
    {
        public int ServiceProviderId { get; set; }
        public int ClientId { get; set; }
        public int PaymentId { get; set; }
        public string Status { get; set; } 
        public int SessionsNumber { get; set; }
        public List<int> SessionIds { get; set; }
    }
}
