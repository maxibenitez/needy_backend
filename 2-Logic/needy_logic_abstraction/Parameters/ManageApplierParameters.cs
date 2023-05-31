using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class ManageApplierParameters
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int NeedId { get; set; }

        [Required]
        [StringLength(8)]
        [RegularExpression("^[0-9]+$")]
        public string ApplierCI { get; set; }
    }
}
