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

        [Required(ErrorMessage = "La dirección es requerida")]
        public string NeedAddress { get; set; }

        [Required(ErrorMessage = "La modalidad es requerida")]
        [RegularExpression(@"^(Remota|Domicilio|Visita)$")]
        public string Modality { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime NeedDate { get; set; }

        [Required(ErrorMessage = "Las habilidades solicitadas son requeridas")]
        public IEnumerable<int> RequestedSkillsId { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (NeedDate <= DateTime.Today.AddDays(7))
            {
                yield return new ValidationResult("La fecha de la necesidad debe ser al menos 7 días posterior a la fecha de hoy");
            }
        }
    }
}
