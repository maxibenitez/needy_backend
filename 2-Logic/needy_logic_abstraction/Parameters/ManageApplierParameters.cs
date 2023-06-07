using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class ManageApplierParameters
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int NeedId { get; set; }

        [Required(ErrorMessage = "La cédula del aplicador es requerida")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "La cédula debe contener 8 digitos")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "La cédula debe contener solo números")]
        public string ApplierCI { get; set; }
    }
}
