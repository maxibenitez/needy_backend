using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class InsertRatingParameters
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int NeedId { get; set; }

        [Required(ErrorMessage = "Receiver CI is required")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "CI must contain 8 digits")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "CI must contain only numbers")]
        public string ReceiverCI { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1.0, 5.0, ErrorMessage = "Rating must be between 1 and 5")]
        [RegularExpression(@"^([1-5](\.[05])?)$")]
        public double Stars { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        [StringLength(150, ErrorMessage = "Comment must have less than 150 characters")]
        public string Comment { get; set; }
    }
}
