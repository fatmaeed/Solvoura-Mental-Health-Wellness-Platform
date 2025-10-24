using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.CommentDTOs
{
    public class CreateCommentDTO
    {
        public string Body { get; set; }
        public int PostId { get; set; }
        public int? ClientId { get; set; }
        public int? ServiceProviderId { get; set; }
    }
}
