using needy_dataAccess.Interfaces;
using needy_dataAccess.Repositories;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;

namespace needy_logic
{
    public class NeedLogic : INeedLogic
    {
        #region Properties and Fields

        private readonly INeedRepository _needRepository;
        private readonly ISkillRepository _skillRepository;


        #endregion

        #region Builders

        public NeedLogic(INeedRepository needRepository, ISkillRepository skillRepository)
        {
            _needRepository = needRepository;
            _skillRepository = skillRepository;
        }

        #endregion

        #region Implements INeedLogic

        public Task<IEnumerable<Need>> GetNeedsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Need>> GetNeedsBySkillAsync(string skill)
        {
            throw new NotImplementedException();
        }

        public Task<Need> GetNeedByIdAsync(int needId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertNeedAsync(InsertNeedParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateNeedAsync(int needId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteNeedAsync(int needId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApplyNeedAsync(int needId, int applierId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UnapplyNeedAsync(int needId, int applierId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AcceptApplierAsync(int applierId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeclineApplierAsync(int applierId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private async Task<bool> VerifySkillAsync(string skillName)
        {
            Skill skill = await _skillRepository.GetSkillByNameAsync(skillName);
            if(skill == null)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
