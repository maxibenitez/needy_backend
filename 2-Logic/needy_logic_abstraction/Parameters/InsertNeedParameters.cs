using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class InsertNeedParameters : IValidatableObject
    {
        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(150, ErrorMessage = "La descripción no debe superar los 150 caracteres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "La dirección es requerida")]
        public string NeedAddress { get; set; }

        [Required(ErrorMessage = "La modalidad es requerida")]
        [RegularExpression(@"^(Remota|Domicilio|Visita)$")]
        public string Modality { get; set; }

        [Required(ErrorMessage = "La fecha de ayuda es requerida")]
        [DataType(DataType.Date)]
        public DateTime NeedDate { get; set; }

        [Required(ErrorMessage = "Las habilidades solicitadas son requeridas")]
        public IEnumerable<int> RequestedSkillsId { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (NeedDate <= DateTime.Today)
            {
                yield return new ValidationResult("La fecha de ayuda debe ser mayor a la fecha de hoy");
            }
        }
    }
}
