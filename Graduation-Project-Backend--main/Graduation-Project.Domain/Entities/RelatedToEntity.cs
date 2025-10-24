using Graduation_Project.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {

    public class RelatedToEntity : BaseEntity {

        [ForeignKey("illness")]
        public int IllnessId { get; set; }

        public Specialization RelatedTo { get; set; }
        public virtual IllnessEntity Illness { get; set; }
    }
}