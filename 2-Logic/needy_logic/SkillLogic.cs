using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction;

namespace needy_logic
{
    public class SkillLogic : ISkillLogic
    {
        #region Properties and Fields

        private readonly ISkillRepository _skillRepository;

        #endregion

        #region Builders

        public SkillLogic(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        #endregion

        #region Implements ISkillLogic

        public async Task<IEnumerable<Skill>> GetSkillsAsync()
        {
            return await _skillRepository.GetSkillsAsync();
        }

        #endregion
    }
}
