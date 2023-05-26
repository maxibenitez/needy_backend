using needy_dto;

namespace needy_logic_abstraction
{
    public interface ISkillLogic
    {
        Task<IEnumerable<Skill>> GetSkillsAsync();
    }
}
