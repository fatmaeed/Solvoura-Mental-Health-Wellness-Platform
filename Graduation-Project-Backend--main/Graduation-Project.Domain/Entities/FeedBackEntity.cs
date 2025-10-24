using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Domain.Entities {

    [Index(nameof(RevieweeId), nameof(EvaluatorId), nameof(SessionId), IsUnique = true)]
    public class FeedBackEntity : BaseEntity {

        [ForeignKey("Evaluator")]
        public int EvaluatorId { get; set; }

        public virtual required UserEntity Evaluator { get; set; }

        [ForeignKey("Reviewee")]
        public int RevieweeId { get; set; }

        public virtual required UserEntity Reviewee { get; set; }
        public int Rate { get; set; }
        public string? Comment { get; set; }
       // public DateTime Date { get; set; }

        [ForeignKey("Session")]
        public int SessionId { get; set; }

        public virtual required SessionEntity Session { get; set; }
    }
}