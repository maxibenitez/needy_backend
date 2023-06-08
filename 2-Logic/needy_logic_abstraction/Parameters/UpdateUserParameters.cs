using System.ComponentModel.DataAnnotations;

namespace needy_logic_abstraction.Parameters
{
    public class UpdateUserParameters
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Zone { get; set; }

        [StringLength(9, MinimumLength = 9, ErrorMessage = "El teléfono debe contener 9 digitos")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "El teléfono debe contener solo números")]
        public string Phone { get; set; }

        [RegularExpression(@"^(Masculino|Femenino|Otros)$")]
        public string Gender { get; set; }

        [StringLength(150, ErrorMessage = "La descripción no debe superar los 150 caracteres")]
        public string AboutMe { get; set; }

        public IEnumerable<int> SkillsId { get; set; }
    }
}
