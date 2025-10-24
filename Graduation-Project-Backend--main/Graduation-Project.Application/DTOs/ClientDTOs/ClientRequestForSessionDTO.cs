using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.ClientDTOs
{
    public enum SessionActionType
    {
        Cancel = 0,
        Postpone = 1
    }

    public class ClientRequestForSessionDTO
    {
        public int ClientId { get; set; }
        public int SessionId { get; set; }
        public SessionActionType ActionType { get; set; }
        public string? Reason { get; set; }
    }
}
