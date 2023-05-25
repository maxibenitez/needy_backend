using Microsoft.AspNetCore.Http;
using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction;
using needy_logic_abstraction.Parameters;
using System.IdentityModel.Tokens.Jwt;

namespace needy_logic
{
    public class NeedLogic : INeedLogic
    {
        #region Properties and Fields

        private readonly INeedRepository _needRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserLogic _userLogic;

        #endregion

        #region Builders

        public NeedLogic(INeedRepository needRepository, 
            IHttpContextAccessor httpContextAccessor,
            ISkillRepository skillRepository,
            IUserLogic userLogic)
        {
            _needRepository = needRepository;
            _httpContextAccessor = httpContextAccessor;
            _skillRepository = skillRepository;
            _userLogic = userLogic;
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
            string userCI = await GetUserCIFromToken();

            return await _needRepository.InsertNeedAsync(userCI, parameters);
        }

        public async Task<bool> UpdateNeedAsync(int needId, UpdateNeedParameters parameters)
        {
            return await _needRepository.UpdateNeedAsync(needId, parameters);
        }

        public async Task<bool> DeleteNeedAsync(int needId)
        {
            await _needRepository.DeleteNeedAppliersAsync(needId);

            return await _needRepository.DeleteNeedAsync(needId);
        }

        public async Task<bool> ApplyNeedAsync(int needId)
        {
            string applierCI = await GetUserCIFromToken();

            return await _needRepository.ApplyNeedAsync(needId, applierCI);
        }

        public async Task<bool> UnapplyNeedAsync(int needId)
        {
            string applierCI = await GetUserCIFromToken();

            return await _needRepository.DeleteNeedApplierAsync(needId, applierCI);
        }

        public async Task<bool> AcceptApplierAsync(int needId, string applierCI)
        {
            //chequear que el usuario esté aplicado en la need
            var result = await _needRepository.AcceptApplierAsync(needId, applierCI);

            ChangeStatusAsync(needId, "Aceptada");

            return result;
        }

        public async Task<bool> DeclineApplierAsync(int needId, string applierCI)
        {
            return await _needRepository.DeleteNeedApplierAsync(needId, applierCI);
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

        private async Task<string> GetUserCIFromToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var authorizationHeader = httpContext.Request.Headers["Authorization"];
                var token = authorizationHeader.ToString().Replace("Bearer ", string.Empty);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                if (jwtToken.Payload.TryGetValue("CI", out var userCI))
                {
                    return userCI.ToString();
                }
            }

            return null;
        }

        #endregion
    }
}
