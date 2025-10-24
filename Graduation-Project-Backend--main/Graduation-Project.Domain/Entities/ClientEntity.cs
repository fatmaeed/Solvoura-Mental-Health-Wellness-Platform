using Graduation_Project.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Domain.Entities {

    public class ClientEntity : PersonEntity {

        [MaxLength(14), RegularExpression("^[0-9]{14}$")]
        public string? NationalId { get; set; }

        public bool IsAnon { get; set; }
        public bool IsVerified { get; set; }

        [MaxLength(50)]
        public string? UserImagePath { get; set; }

        public string? NationalImagePath { get; set; }

        public string? UserAndNationalImagePath { get; set; }

        [Phone, RegularExpression("^01[0-2,5]{1}[0-9]{8}$")]
        public string? AlternativePhoneNumber { get; set; }

        [MaxLength(50)]
        public string? HistoryIllness { get; set; }

        public Specialization? NeededServices { get; set; }
    }
}