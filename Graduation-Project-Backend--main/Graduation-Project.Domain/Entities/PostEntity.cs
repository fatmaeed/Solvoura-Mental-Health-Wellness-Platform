using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Domain.Entities
{
    public class PostEntity: BaseEntity
    {
        public string body { get; set; }
        public string? imagePath { get; set; }
        [ForeignKey("ServiceProvider")]
        public int? ServiceProviderId { get; set; }

        public virtual ServiceProviderEntity? ServiceProvider { get; set; }

        [ForeignKey("Client")]
        public int? ClientId { get; set; }

        public virtual ClientEntity? Client { get; set; }
        public virtual ICollection<CommentEntity>? Comments { get; set; }

        public int Likes { get; set; } = 0;
       public virtual UserLikes? UserLikes {  get; set; }
        public DateTime?  PostDate { get; set; }

    }
}
