using Graduation_Project.Application.DTOs.SymptomDTOs;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Application.DTOs.IllnessDTOs {

    public class DisplayIllnessDTO {
        public int Id { get; set; }

        [MaxLength(70)]
        public required string IllnessName { get; set; }

        [MaxLength(500)]
        public required string IllnessDescription { get; set; }

        public required string? Level { get; set; }
        public required string Image { get; set; }
        public required List<string?> RelatedSpecialties { get; set; }
        public required List<DisplaySymptomDTO> Symptoms { get; set; }
    }
}