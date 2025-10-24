using Graduation_Project.Application.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Application.DTOs.AccountDTOs {

    public class PersonDTO : RegisterUserDTO {

        [MaxLength(20)]
        public string FirstName { get; set; }

        [MaxLength(20)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string Address { get; set; }

        [Range(0, 2)]
        public int Gender { get; set; }

        [Birthdate]
        public DateOnly BirthDate { get; set; }
    }
}