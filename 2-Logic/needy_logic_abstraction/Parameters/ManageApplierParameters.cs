using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class ManageApplierParameters
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int NeedId { get; set; }

        [Required(ErrorMessage = "Applier CI is required")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "CI must contain 8 digits")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "CI must contain only numbers")]
        public string ApplierCI { get; set; }
    }
}
