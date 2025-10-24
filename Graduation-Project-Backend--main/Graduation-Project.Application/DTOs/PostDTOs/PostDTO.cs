using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.DTOs.PostDTOs
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string? ImagePath { get; set; }
        public int? ServiceProviderId { get; set; }
        public int? ClientId { get; set; }
        public string? Date {  get; set; }
        public int Likes { get; set; }
        public bool? IsLikedByCurrentUser { get; set; }
        public int? UserLikeId { get; set; }

    }
}
