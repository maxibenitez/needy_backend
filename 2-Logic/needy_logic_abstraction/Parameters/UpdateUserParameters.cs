using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class UpdateUserParameters
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Zone { get; set; }

        [StringLength(9)]
        [RegularExpression(@"^09\d{7}$", ErrorMessage = "El teléfono debe contener solo números")]
        public string Phone { get; set; }

        [RegularExpression(@"^(Masculino|Femenino|Otros)$")]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [StringLength(150, ErrorMessage = "La descripción no debe superar los 150 caracteres")]
        public string AboutMe { get; set; }
    }
}
