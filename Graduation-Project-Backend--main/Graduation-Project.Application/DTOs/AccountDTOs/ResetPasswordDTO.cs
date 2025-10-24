using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Application.DTOs.AccountDTOs {

    public class ResetPasswordDTO {
        [Required, MinLength(8), DataType(DataType.Password)]
        public required string Password { get; set; }

        [Compare("Password")]
        public required string ConfirmPassword { get; set; }

        public required string Email { get; set; }

        [Required, MinLength(6)]
        public required string Token { get; set; }
    }
}