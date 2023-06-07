using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class LoginParameters
    {
        [EmailAddress]
        [Required(ErrorMessage = "El email es requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; }
    }
}
