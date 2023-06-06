using Microsoft.IdentityModel.Tokens;
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
        private readonly IUserLogic _userLogic;
        private readonly ITokenLogic _tokenLogic;

        #endregion

        #region Builders

        public NeedLogic(INeedRepository needRepository, 
            ISkillRepository skillRepository,
            IUserLogic userLogic,
            ITokenLogic tokenLogic)
        {
            _needRepository = needRepository;
            _skillRepository = skillRepository;
            _userLogic = userLogic;
            _tokenLogic = tokenLogic;
        }

        #endregion

        #region Implements INeedLogic

        public async Task<IEnumerable<Need>> GetNeedsAsync()
        {
            List<NeedData> data = (await _needRepository.GetNeedsAsync()).ToList();

            if (data != null)
            {
                List<Need> needs = new List<Need>();

                foreach (NeedData need in data)
                {
                    needs.Add(await NeedBuilderAsync(need));
                }

                return needs;
            }

            return null;
        }

        public async Task<IEnumerable<Need>> GetNeedsBySkillAsync(int skillId)
        {
            List<NeedData> data = (await _needRepository.GetNeedsBySkillAsync(skillId)).ToList();

            if (data != null)
            {
                List<Need> needs = new List<Need>();

                foreach(NeedData need in data)
                {
                    needs.Add(await NeedBuilderAsync(need));
                }

                return needs;
            }

            return null;
        }

        public async Task<Need> GetNeedByIdAsync(int needId)
        {
            NeedData data = await _needRepository.GetNeedByIdAsync(needId);

            if (data != null)
            {
                return await NeedBuilderAsync(data);
            }

            return null;  
        }

        public async Task<bool> InsertNeedAsync(InsertNeedParameters parameters)
        {
            string userCI = await _tokenLogic.GetUserCIFromToken();

            return await _needRepository.InsertNeedAsync(userCI, parameters);
        }

        public async Task<bool> UpdateNeedAsync(UpdateNeedParameters parameters)
        {
            return await _needRepository.UpdateNeedAsync(parameters);
        }

        public async Task<bool> DeleteNeedAsync(int needId)
        {
            await _needRepository.DeleteNeedAppliersAsync(needId);

            return await _needRepository.DeleteNeedAsync(needId);
        }

        public async Task<bool> ApplyNeedAsync(int needId)
        {
            string applierCI = await _tokenLogic.GetUserCIFromToken();

            if (!await IsNeedRequestor(needId, applierCI))
            {
                return await _needRepository.ApplyNeedAsync(needId, applierCI);
            }

            return false;
        }

        public async Task<bool> UnapplyNeedAsync(int needId)
        {
            string applierCI = await _tokenLogic.GetUserCIFromToken();

            return await _needRepository.DeleteNeedApplierAsync(needId, applierCI);
        }

        public async Task<bool> AcceptApplierAsync(ManageApplierParameters parameters)
        {
            if(!await ExistAcceptedApplierAsync(parameters.NeedId))
            {
                var appliersCI = await _needRepository.GetNeedAppliersAsync(parameters.NeedId);

                if(appliersCI.Contains(parameters.ApplierCI))
                {
                    await ChangeStatusAsync(parameters.NeedId, "Aceptada");

                    return await _needRepository.AcceptApplierAsync(parameters);
                }
            }

            return false;
        }

        public async Task<bool> DeclineApplierAsync(ManageApplierParameters parameters)
        {
            return await _needRepository.DeleteNeedApplierAsync(parameters.NeedId, parameters.ApplierCI);
        }

        #endregion

        #region Private Methods

        private async Task<Need> NeedBuilderAsync (NeedData data)
        {
            var need = new Need
            {
                Id = data.Id,
                Status = data.Status,
                Description = data.Description,
                CreationDate = data.CreationDate,
                NeedDate = data.NeedDate,
                AcceptedDate = data.AcceptedDate,
                NeedAddress = data.NeedAddress,
                Modality = data.Modality,
            };

            need.RequestedSkill = await _skillRepository.GetSkillByIdAsync(data.RequestedSkillId);
            need.Appliers = await GetNeedAppliersAsync(data.Id);
            need.Requestor = await _userLogic.GetUserByCIAsync(data.RequestorCI);

            if (data.AcceptedApplierCI is not null)
            {
                need.AcceptedApplier = await _userLogic.GetUserByCIAsync(data.AcceptedApplierCI);
            }

            return need;
        }

        private async Task<bool> ChangeStatusAsync(int needId, string status)
        {
            return await _needRepository.ChangeStatusAsync(needId, status);
        }

        private async Task<IEnumerable<User>> GetNeedAppliersAsync(int needId)
        {
            var appliersCI = await _needRepository.GetNeedAppliersAsync(needId);

            List<User> appliers = new List<User>();

            foreach (string applierCI in appliersCI)
            {
                appliers.Add(await _userLogic.GetUserByCIAsync(applierCI));
            }

            return appliers;
        }

        private async Task<bool> ExistAcceptedApplierAsync(int needId)
        {
            string acceptedApplierCI = await _needRepository.GetNeedAcceptedApplierAsync(needId);

            return acceptedApplierCI.IsNullOrEmpty() ? false : true;
        }

        private async Task<bool> IsNeedRequestor(int needId, string applierCI)
        {
            string requestorCI = await _needRepository.GetNeedRequestorAsync(needId);

            return requestorCI.Equals(applierCI) ? true : false;
        }

        #endregion
    }
}
