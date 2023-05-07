using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;

namespace needy_logic
{
    public class NeedLogic : INeedLogic
    {
        #region Properties and Fields

        private readonly INeedRepository _needRepository;

        #endregion

        #region Builders

        public NeedLogic(INeedRepository needRepository)
        {
            _needRepository = needRepository;
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
    }
}
