using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class InsertRatingParameters
    {
        [Range(1, int.MaxValue)]
        public int NeedId { get; set; }

        [Required]
        [StringLength(8)]
        [RegularExpression("^[0-9]+$")]
        public string ReceiverCI { get; set; }

        [Required(ErrorMessage = "La calificación es requerida")]
        [Range(1.0, 5.0, ErrorMessage = "La calificación debe estar entre 1 y 5")]
        public double Stars { get; set; }

        [Required(ErrorMessage = "El comentario es requerido")]
        [StringLength(150, ErrorMessage = "El comentario no debe superar los 150 caracteres")]
        public string Comment { get; set; }
    }
}
