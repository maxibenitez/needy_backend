using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class InsertNeedParameters : IValidatableObject
    {
        [StringLength(150, ErrorMessage = "La descripción no debe superar los 150 caracteres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "La fecha de ayuda es requerida")]
        [DataType(DataType.Date)]
        public DateTime NeedDate { get; set; }

        [Required(ErrorMessage = "La habilidad solicitada requerida")]
        [Range(1, int.MaxValue)]
        public int RequestedSkillId { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (NeedDate <= DateTime.Today)
            {
                yield return new ValidationResult("La fecha de ayuda debe ser mayor a la fecha de hoy");
            }
        }
    }
}
