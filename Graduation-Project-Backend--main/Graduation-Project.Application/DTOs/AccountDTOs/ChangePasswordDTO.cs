using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Application.DTOs.AccountDTOs {

    public class ChangePasswordDTO {
        public int UserId { get; set; }
        public required string OldPassword { get; set; }
        [Required, MinLength(8), DataType(DataType.Password)]

        public required string NewPassword { get; set; }

        [Compare("NewPassword")]
        public required string ConfirmPassword { get; set; }
    }
}