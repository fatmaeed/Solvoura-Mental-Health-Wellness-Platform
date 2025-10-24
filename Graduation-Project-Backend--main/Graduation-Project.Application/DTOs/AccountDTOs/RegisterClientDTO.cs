using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Application.DTOs.AccountDTOs {

    public class RegisterClientDTO : PersonDTO {

        [MaxLength(14), RegularExpression("^[0-9]{14}$"), MinLength(14)]
        public string? NationalNumber { get; set; }

        public IFormFile? UserImage { get; set; }

        public IFormFile? NationalImage { get; set; }

        public IFormFile? UserAndNationalImage { get; set; }

        [Range(0, 6)]
        public int? NeededSpecilization { get; set; }

        [MaxLength(150)]
        public string? IllnessesHistory { get; set; } // IllnessesHistory

        [MaxLength(11), Phone, RegularExpression("^01[0-2,5]{1}[0-9]{8}$"), MinLength(11)]
        public string? SecondPhoneNumber { get; set; }
    }
}