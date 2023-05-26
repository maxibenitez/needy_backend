using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class InsertRatingParameters
    {
        [Range(1, int.MaxValue)]
        public int NeedId { get; set; }

        [Required]
        public string ReceiverCI { get; set; }

        [Range(0.5, 5.0, ErrorMessage = "Calificación debe estar entre 0.5 y 5")]
        public decimal Stars { get; set; }

        [StringLength(150, ErrorMessage = "Comentario no debe superar los 150 caracteres")]
        public string? Comment { get; set; }
    }
}
