using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Application.Common.Attributes {

    public class BirthdateAttribute : ValidationAttribute {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
            if (value is not DateOnly date) {
                return new ValidationResult("Invalid date format.");
            }

            var now = DateOnly.FromDateTime(DateTime.Now);

            if (date < now.AddYears(-5) && date > now.AddYears(-100)) {
                return ValidationResult.Success;
            }

            return new ValidationResult("Date must be between 5 and 100 years ago.");
        }
    }
}