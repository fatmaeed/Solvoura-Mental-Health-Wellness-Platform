using Graduation_Project.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {

    public class SymptomEntity : BaseEntity {
        public LevelsType Levels { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [ForeignKey("Illness")]
        public int IllnessId { get; set; }

        public virtual IllnessEntity Illness { get; set; }
    }
}