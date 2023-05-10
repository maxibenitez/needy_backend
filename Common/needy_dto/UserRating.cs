using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace needy_dto
{
    public class UserRating
    {
        public decimal Average { get; set; }

        public IEnumerable<string> Comments { get; set; }
    }
}
