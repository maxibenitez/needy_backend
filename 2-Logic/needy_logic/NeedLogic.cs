using Microsoft.IdentityModel.Tokens;
using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Enumerables;
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
            List<Need> needs = new List<Need>();

            foreach(NeedData need in data)
            {
                needs.Add(await NeedBuilderAsync(need));
            }

            return needs;
        }

        public async Task<IEnumerable<Need>> GetNeedsBySkillNameAsync(string skillName)
        {
            List<NeedData> data = (await _needRepository.GetNeedsBySkillNameAsync(skillName)).ToList();
            List<Need> needs = new List<Need>();

            foreach (NeedData need in data)
            {
                needs.Add(await NeedBuilderAsync(need));
            }

            return needs;
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

        public async Task<IEnumerable<Need>> GetUserCreatedNeedsAsync(string userCI)
        {
            List<NeedData> data = (await _needRepository.GetUserCreatedNeedsAsync(userCI)).ToList();

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

        public async Task<IEnumerable<Need>> GetUserAppliedNeedsAsync(string userCI)
        {
            List<NeedData> data = (await _needRepository.GetUserAppliedNeedsAsync(userCI)).ToList();

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

        public async Task<bool> InsertNeedAsync(InsertNeedParameters parameters)
        {
            string userCI = await _tokenLogic.GetUserCIFromToken();

            int needId = await _needRepository.InsertNeedAsync(userCI, parameters);

            if (needId != 0)
            {
                foreach (int skillId in parameters.RequestedSkillsId)
                {
                    await _needRepository.InsertNeedSkillAsync(needId, skillId);
                }

                return true;
            }

            return false;
        }

        public async Task<bool> UpdateNeedAsync(UpdateNeedParameters parameters)
        {
            if (await _needRepository.UpdateNeedAsync(parameters))
            {
                if (await _needRepository.DeleteNeedSkillsAsync(parameters.NeedId))
                {
                    foreach (int skillId in parameters.RequestedSkillsId)
                    {
                        await _needRepository.InsertNeedSkillAsync(parameters.NeedId, skillId);
                    }

                    return true;
                }

                return false;
            }

            return false;
        }

        public async Task<bool> DeleteNeedAsync(int needId)
        {
            await _needRepository.DeleteNeedAppliersAsync(needId);

            return await _needRepository.DeleteNeedAsync(needId);
        }

        public async Task<ErrorStatus> ApplyNeedAsync(int needId)
        {
            string applierCI = await _tokenLogic.GetUserCIFromToken();

            bool isUserHasRequiredSkills = await IsUserHasRequiredSkills(needId, applierCI);
            bool isNeedRequestor = await IsNeedRequestor(needId, applierCI);

            if (isNeedRequestor)
            {
                return ErrorStatus.IsNeedRequestor;
            }

            if (!isUserHasRequiredSkills)
            {
                return ErrorStatus.NotHasRequiredSkills;
            }
    
            if(!await _needRepository.ApplyNeedAsync(needId, applierCI))
            {
                return ErrorStatus.InternalServerError;
            }

            return ErrorStatus.Success;
        }

        public async Task<bool> UnapplyNeedAsync(int needId)
        {
            string applierCI = await _tokenLogic.GetUserCIFromToken();

            return await _needRepository.DeleteNeedApplierAsync(needId, applierCI);
        }

        public async Task<ErrorStatus> AcceptApplierAsync(ManageApplierParameters parameters)
        {
            if(!await ExistAcceptedApplierAsync(parameters.NeedId))
            {
                var appliersCI = await _needRepository.GetNeedAppliersAsync(parameters.NeedId);

                if(appliersCI.Contains(parameters.ApplierCI))
                {
                    if(await ChangeStatusAsync(parameters.NeedId, "Accepted") &&
                        await _needRepository.AcceptApplierAsync(parameters))
                    {
                        return ErrorStatus.Success;
                    }

                    return ErrorStatus.InternalServerError;
                }

                return ErrorStatus.ApplierNotExist;
            }

            return ErrorStatus.AcceptedApllierExist;
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
                Title = data.Title,
                Status = data.Status,
                Description = data.Description,
                CreationDate = data.CreationDate,
                NeedDate = data.NeedDate,
                AcceptedDate = data.AcceptedDate,
                NeedAddress = data.NeedAddress,
                Modality = data.Modality,
            };

            need.RequestedSkills = await _skillRepository.GetNeedSkillsAsync(data.Id);
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

        private async Task<bool> IsUserHasRequiredSkills(int needId, string applierCI)
        {
            var needSkills = (await _skillRepository.GetNeedSkillsAsync(needId)).Select(skill => skill.Id).ToList();
            var userSkills = (await _skillRepository.GetUserSkillsAsync(applierCI)).Select(skill => skill.Id).ToList();

            return needSkills.Any(skillId => userSkills.Contains(skillId)) ? true : false;
        }

        #endregion
    }
}
