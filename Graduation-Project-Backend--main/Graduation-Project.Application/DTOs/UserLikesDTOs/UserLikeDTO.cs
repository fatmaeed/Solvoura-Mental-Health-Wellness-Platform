using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Domain.Entities;

namespace Graduation_Project.Application.DTOs.UserLikesDTOs
{
    public class UserLikeDTO
    {
        public int Id { get; set; } 
        [Required]
        public int? UserId { get; set; }
        [Required]
        public int? PostId { get; set; }
        [Required]
        public bool Isliked { get; set; }
    }
}
