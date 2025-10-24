using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.OurServiceDTOs
{
    public class DisplayServices
    {
        public int id { get; set; }
        public string title { get; set; }
        public  string icon { get; set; }
        public string description { get; set; }


    }
}
