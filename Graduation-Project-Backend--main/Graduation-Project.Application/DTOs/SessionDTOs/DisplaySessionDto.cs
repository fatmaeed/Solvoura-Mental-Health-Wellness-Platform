using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.SessionDTOs
{
    public class DisplaySessionDto
    {

            public int Id { get; set; }
            public DateTime StartDateTime { get; set; }
            public string Duration { get; set; }
            public int Type { get; set; }
            public int Status { get; set; }
        }

    
}
