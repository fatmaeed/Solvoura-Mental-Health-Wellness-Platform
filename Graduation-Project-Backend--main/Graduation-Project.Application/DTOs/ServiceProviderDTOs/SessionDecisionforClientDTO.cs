using Graduation_Project.Application.DTOs.ClientDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.ServiceProviderDTOs
{
    public class SessionDecisionforClientDTO
    {
        public int SessionId { get; set; }

        public bool IsApproved { get; set; }

        public SessionActionType ClientRequetStatus { get; set; }
        public DateTime? NewStartDateTime { get; set; }

        public string? Notes { get; set; }
    }
}
