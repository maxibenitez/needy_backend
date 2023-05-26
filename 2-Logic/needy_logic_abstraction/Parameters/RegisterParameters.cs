using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class RegisterParameters
    {
        [Required(ErrorMessage = "Cédula requerida")]
        public string CI { get; set; }

        [Required(ErrorMessage = "Nombre requerido")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Apellido requerido")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Dirección requerida")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Zona requerida")]
        public string Zone { get; set; }

        [Required(ErrorMessage = "Teléfono requerido")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Género requerido")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Fecha de nacimiento requerida")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Email requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contraseña requerida")]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "La contraseña debe contener entre 4 y 8 caracteres")]
        public string Password { get; set; }
    }
}
