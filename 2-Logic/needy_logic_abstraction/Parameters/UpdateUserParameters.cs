using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class UpdateUserParameters
    {
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

        [Required(ErrorMessage = "About Me is required")]
        [StringLength(150, ErrorMessage = "About Me must have less than 150 characters")]
        public string AboutMe { get; set; }

        public IEnumerable<int> SkillsId { get; set; }
    }
}
