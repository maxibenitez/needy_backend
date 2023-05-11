using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_dataAccess.Interfaces
{
    public interface ISkillRepository
    {
        Task<IEnumerable<Skill>> GetSkills();
        Task<Skill> GetSkillByNameAsync(string name);
    }
}
