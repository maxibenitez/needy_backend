using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class UpdateNeedParameters : IValidatableObject
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int NeedId { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "Description must have less than 150 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Need Address is required")]
        public string NeedAddress { get; set; }

        [Required(ErrorMessage = "Modality is required")]
        [RegularExpression(@"^(Remote|Home|Visit)$")]
        public string Modality { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime NeedDate { get; set; }

        [Required(ErrorMessage = "Skills are required")]
        public IEnumerable<int> RequestedSkillsId { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (NeedDate <= DateTime.Today.AddDays(7))
            {
                yield return new ValidationResult("Need Date must be at least 7 days after today's date");
            }
        }
    }
}
