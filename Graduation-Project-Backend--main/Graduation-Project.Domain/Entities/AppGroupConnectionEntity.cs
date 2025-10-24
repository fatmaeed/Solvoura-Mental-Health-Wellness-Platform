using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {

    [Index(nameof(UserId), nameof(GroupName), IsUnique = true)]
    public class AppGroupConnectionEntity : BaseEntity {

        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual UserEntity? User { get; set; }
        public string GroupName { get; set; }
    }
}