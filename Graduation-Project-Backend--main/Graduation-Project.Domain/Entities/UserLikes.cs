using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Domain.Entities
{
    public class UserLikes : BaseEntity
    {
        
        public int? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual UserEntity? User { get; set; }
        public int? PostId { get; set; }
        [ForeignKey(nameof(PostId))]
        public virtual PostEntity? Post { get; set; }

        public bool Isliked { get; set; }

    }
}
