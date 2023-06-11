using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class RegisterParameters : IValidatableObject
    {
        [Required(ErrorMessage = "CI is required")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "CI must contain 8 digits")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "CI must contain only numbers")]
        public string CI { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Zone is requireda")]
        public string Zone { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Phone must contain 9 digits")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Phone must contain only numbers")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression(@"^(Male|Female|Other)$")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "BirthDate is required")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Password must have more than 4 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "About Me is required")]
        [StringLength(150, ErrorMessage = "About Me must have less than 150 characters")]
        public string AboutMe { get; set; }

        public IEnumerable<int> SkillsId { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            DateTime currentDate = DateTime.Today;
            int age = currentDate.Year - BirthDate.Year;

            if (BirthDate > currentDate.AddYears(-age))
            {
                age--;
            }

            if (age < 18)
            {
                yield return new ValidationResult("You must be over 18 years old");
            }
        }
    }
}
