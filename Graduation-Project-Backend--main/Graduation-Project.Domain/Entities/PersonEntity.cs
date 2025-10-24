using Graduation_Project.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {

    public class PersonEntity : BaseEntity {

        [MaxLength(20)]
        public string FirstName { get; set; }

        [MaxLength(20)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string Address { get; set; }

        public Gender Gender { get; set; }

        public DateOnly BirthDate { get; set; }

        [ForeignKey("Id")]
        public virtual UserEntity User { get; set; }
    }
}