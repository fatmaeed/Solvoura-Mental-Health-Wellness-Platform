using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.OurServiceDTOs
{
    public class OurserviceDTO
    {
        [MaxLength(50,ErrorMessage ="Title Length must be less than from 50 ")]
        public string Title {  get; set; }
        public string Icon { get; set; } 
        [MaxLength(200, ErrorMessage = "Title Length must be less than from 200 ")]
        public string Description { get; set; }
    }
}
