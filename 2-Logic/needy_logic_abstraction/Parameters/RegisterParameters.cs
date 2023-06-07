using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class RegisterParameters : IValidatableObject
    {
        [Required(ErrorMessage = "La cédula requerida")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "La cédula debe contener 8 digitos")]
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
        [StringLength(9, MinimumLength = 9, ErrorMessage = "El teléfono debe contener 9 digitos")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "El teléfono debe contener solo números")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El género requerido")]
        [RegularExpression(@"^(Masculino|Femenino|Otros)$")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento requerida")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "La contraseña debe contener entre 4 y 8 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "La descripción es requerido")]
        [StringLength(150, ErrorMessage = "La descripción no debe superar los 150 caracteres")]
        public string AboutMe { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            DateTime currentDate = DateTime.Today;
            int age = currentDate.Year - BirthDate.Year;

            if (BirthDate > currentDate.AddYears(-age))
            {
                age--;
            }

            if (age < 18)
            {
                yield return new ValidationResult("Debe ser mayor de 18 años");
            }
        }
    }
}
