using needy_dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace needy_logic_abstraction
{
    public interface ISkillLogic
    {
        Task<IEnumerable<Skill>> GetSkillsAsync();
    }
}
