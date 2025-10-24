using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.ServiceProviderDTOs
{
    public class PosponedandcanceledSessionDTO
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public decimal SessionPrice { get; set; }
        public string Type { get; set; } 
        public string Status { get; set; }
        public int ClientId { get; set; }
        public string ClientName{get; set;}

    }
}
