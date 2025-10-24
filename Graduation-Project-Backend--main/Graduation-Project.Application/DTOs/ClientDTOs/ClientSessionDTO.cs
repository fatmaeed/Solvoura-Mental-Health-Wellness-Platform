using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.ClientDTOs
{
    public class ClientSessionDTO
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public string Doctorspecialization { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public decimal SessionPrice { get; set; }
        public  string UserImagePath { get; set; }
        public string Type { get; set; } // Online or Offline
        public string Status { get; set; } // NotStarted, Done, Canceled, etc.
        public bool CanCancelOrPostpone { get; set; }
    }
}
