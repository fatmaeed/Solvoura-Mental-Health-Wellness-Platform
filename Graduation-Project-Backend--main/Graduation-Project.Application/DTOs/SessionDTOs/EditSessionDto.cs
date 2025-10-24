using Graduation_Project.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.SessionDTOs
{
    public class EditSessionDto
    {
        public int Id { get; set; }
        public int ServerProvider { get; set; } 
        public DateTime StartDateTime { get; set; }
        public TimeSpan Duration { get; set; }

        public SessionStatus Status { get; set; }
        public SessionType Type { get; set; }
        //public decimal SessionPrice { get; set; }

    }
}
