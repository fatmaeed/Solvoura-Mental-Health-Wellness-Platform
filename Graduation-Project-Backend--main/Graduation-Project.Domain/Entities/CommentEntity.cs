using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Domain.Entities
{
    public class CommentEntity: BaseEntity
    {
        public string Body { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public virtual PostEntity Post { get; set; }

        [ForeignKey("Client")]
        public int? ClientId { get; set; }
        public virtual  ClientEntity? Client { get; set; }
        [ForeignKey("ServiceProvider")]
        public int? ServiceProviderId { get; set; }

        public virtual ServiceProviderEntity? ServiceProvider { get; set; }
    }
}
