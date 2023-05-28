using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class RegisterParameters
    {
        [Required(ErrorMessage = "La cédula requerida")]
        [StringLength(8)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "La cédula debe contener solo números")]
        public string CI { get; set; }

        [Required(ErrorMessage = "El nombre requerido")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido requerido")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "La dirección requerida")]
        public string Address { get; set; }

        [Required(ErrorMessage = "La zona requerida")]
        public string Zone { get; set; }

        [Required(ErrorMessage = "El teléfono requerido")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El género requerido")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento requerida")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "El email requerido")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña requerida")]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "La contraseña debe contener entre 4 y 8 caracteres")]
        public string Password { get; set; }
    }
}
