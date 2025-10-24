using Graduation_Project.Domain.Enums;

namespace Graduation_Project.Domain.Entities {

    public class IllnessEntity : BaseEntity {
        public string Title { get; set; }
        public string Description { get; set; }
        public LevelsType Level { get; set; }
        public string ImagePath { get; set; }
        public virtual List<SymptomEntity> Symptoms { get; set; }
        public virtual List<RelatedToEntity> related { get; set; }
    }
}