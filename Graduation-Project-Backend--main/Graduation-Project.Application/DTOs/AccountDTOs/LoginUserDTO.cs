using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Application.DTOs.AccountDTOs {

    public class LoginUserDTO {

        [MinLength(6), MaxLength(50)]
        public required string UserNameOrEmail { get; set; }

        public required string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}