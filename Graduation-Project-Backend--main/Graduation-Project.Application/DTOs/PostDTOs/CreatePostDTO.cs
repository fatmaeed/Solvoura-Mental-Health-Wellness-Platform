using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.PostDTOs
{
    public class CreatePostDTO
    {
        public string Body { get; set; }
        public string? ImagePath { get; set; }
        public int? ServiceProviderId { get; set; }
        public int? ClientId { get; set; }
    }
}
