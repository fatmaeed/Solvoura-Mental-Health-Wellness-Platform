using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Application.DTOs.FeedBackDTOs {
    public class CreateFeedBackDTO {
        [Required]
        public int EvaluatorId { get; set; }
        //[Required]
        //public int RevieweeId { get; set; }
        [Required]
        public int SessionId { get; set; }
        [Required, Range(1, 5)]
        public int Rate { get; set; }
        public string? Comment { get; set; }

    }
}
