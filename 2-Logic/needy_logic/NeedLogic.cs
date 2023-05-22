using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;
using Npgsql;
using System.Data;
using System.Reflection.PortableExecutable;

namespace needy_logic
{
    public class NeedLogic : INeedLogic
    {
        #region Properties and Fields

        private readonly INeedRepository _needRepository;
        private readonly IUserRepository _userRepository;
        //private readonly ISkillRepository _skillRepository;

        #endregion

        #region Builders

        public NeedLogic(INeedRepository needRepository, IUserRepository userRepository)//, ISkillRepository skillRepository)
        {
            _needRepository = needRepository;
            _userRepository = userRepository;
            //_skillRepository = skillRepository;
        }

        #endregion

        #region Implements INeedLogic

        public async Task<IEnumerable<Need>> GetNeedsAsync()
        {
            //return await _needRepository.GetNeedsAsync();
            return null;
        }

        public async Task<IEnumerable<Need>> GetNeedsBySkillAsync(int skillId)
        {
            //return await _needRepository.GetNeedsBySkillAsync(skillId);
            return null;
        }

        public async Task<Need> GetNeedByIdAsync(int needId)
        {
            NeedData data = await _needRepository.GetNeedByIdAsync(needId);

            if (data != null)
            {
                var need = new Need
                {
                    Id = data.Id,
                    Status = data.Status,
                    Description = data.Description,
                    CreationDate = data.CreationDate,
                    NeedDate = data.NeedDate,
                    AcceptedDate = data.AcceptedDate,
                };

                //need.RequestedSkill = await _skillRepository.GetSkillById(data.RequestedSkillId);
                need.Requestor = await _userRepository.GetUserByCIAsync(data.RequestorCI);

                if (data.AppliersCI != null)
                {
                    need.Appliers = await GetNeedAppliersAsync(data.AppliersCI);
                }

                if (data.AcceptedApplierCI != null)
                {
                    need.AcceptedApplier = await _userRepository.GetUserByCIAsync(data.AcceptedApplierCI);
                }

                return need;
            }

            return null;  
        }

        public async Task<bool> InsertNeedAsync(InsertNeedParameters parameters)
        {
            return await _needRepository.InsertNeedAsync(parameters);
        }

        public async Task<bool> UpdateNeedAsync(int needId, UpdateNeedParameters parameters)
        {
            return await _needRepository.UpdateNeedAsync(needId, parameters);
        }

        public async Task<bool> DeleteNeedAsync(int needId)
        {
            return await _needRepository.DeleteNeedAsync(needId);
        }

        public async Task<bool> ApplyNeedAsync(int needId, string applierCi)
        {
            return await _needRepository.ApplyNeedAsync(needId, applierCi);
        }

        public async Task<bool> UnapplyNeedAsync(int needId, string applierCi)
        {
            return await _needRepository.UnapplyNeedAsync(needId, applierCi);
        }

        public async Task<bool> AcceptApplierAsync(int needId, string applierCi)
        {
            return await _needRepository.AcceptApplierAsync(needId, applierCi);
        }

        public async Task<bool> DeclineApplierAsync(int needId, string applierCi)
        {
            return await _needRepository.DeclineApplierAsync(needId, applierCi);
        }

        #endregion

        #region Private Methods

        private async Task<bool> ChangeStatusAsync(int needId, string status)
        {
            return await _needRepository.ChangeStatusAsync(needId, status);
        }

        private async Task<IEnumerable<User>> GetNeedAppliersAsync(IEnumerable<string> appliersCI)
        {
            List<User> appliers = new List<User>();

            foreach (string applierCI in appliersCI)
            {
                appliers.Add(await _userRepository.GetUserByCIAsync(applierCI));
            }

            return appliers;
        }

        #endregion
    }
}
