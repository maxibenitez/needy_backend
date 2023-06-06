using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class UpdateNeedParameters : IValidatableObject
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int NeedId { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "Descripción no debe superar los 150 caracteres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "La dirección requerida")]
        public string NeedAddress { get; set; }

        [Required(ErrorMessage = "La modalidad es requerida")]
        [RegularExpression(@"^(Remota|Domicilio|Visita)$")]
        public string Modality { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime NeedDate { get; set; }

        [Required(ErrorMessage = "Habilidad solicitada requerida")]
        [Range(1, int.MaxValue)]
        public int RequestedSkillId { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (NeedDate <= DateTime.Today)
            {
                yield return new ValidationResult("Fecha de necesidad debe ser mayor a la fecha de hoy");
            }
        }
    }
}
