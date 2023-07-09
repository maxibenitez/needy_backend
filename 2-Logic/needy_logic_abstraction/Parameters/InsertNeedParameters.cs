using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class InsertNeedParameters : IValidatableObject
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(20, ErrorMessage = "Title must have less than 20 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(150, ErrorMessage = "Description must have less than 150 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Need Address is required")]
        public string NeedAddress { get; set; }

        [Required(ErrorMessage = "Need Zone is required")]
        public string NeedZone { get; set; }

        [Required(ErrorMessage = "Modality is required")]
        [RegularExpression(@"^(Remote|Home|Visit)$")]
        public string Modality { get; set; }

        [Required(ErrorMessage = "Need Date is required")]
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
