using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Application.DTOs.AccountDTOs {

    public class RegisterUserDTO {

        [MaxLength(40)]
        public string? UserName { get; set; }

        [MaxLength(40), RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [MaxLength(11), RegularExpression("^01[0125][0-9]{8}$", ErrorMessage = "Phone number is not valid")]
        public string PhoneNumber { get; set; }
    }
}