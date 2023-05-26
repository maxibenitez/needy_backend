using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class LoginParameters
    {
        [Required(ErrorMessage = "Email requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contraseña requerida")]
        public string Password { get; set; }
    }
}
