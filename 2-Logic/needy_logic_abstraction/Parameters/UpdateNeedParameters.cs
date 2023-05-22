using needy_dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace needy_logic_abstraction.Parameters
{
    public class UpdateNeedParameters
    {
        public string Description { get; set; }

        public DateTime NeedDate { get; set; }

        public int RequestedSkillId { get; set; }
    }
}
