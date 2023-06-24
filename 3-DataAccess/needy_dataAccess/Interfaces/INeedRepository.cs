using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_dataAccess.Interfaces
{
    public interface INeedRepository
    {
        Task<IEnumerable<NeedData>> GetNeedsAsync();

        Task<IEnumerable<NeedData>> GetNeedsBySkillAsync(int skillId);

        Task<IEnumerable<NeedData>> GetNeedsBySkillNameAsync(string skillName);

        Task<NeedData> GetNeedByIdAsync(int needId);

        Task<IEnumerable<string>> GetNeedAppliersAsync(int needId);

        Task<string> GetNeedRequestorAsync(int needId);

        Task<string> GetNeedAcceptedApplierAsync(int needId);

        Task<IEnumerable<NeedData>> GetUserCreatedNeedsAsync(string userCI);

        Task<IEnumerable<NeedData>> GetUserAppliedNeedsAsync(string userCI);

        Task<int> InsertNeedAsync(string userCI, InsertNeedParameters parameters);

        Task<bool> InsertNeedSkillAsync(int needId, int skillId);

        Task<bool> DeleteNeedSkillsAsync(int needId);

        Task<bool> UpdateNeedAsync(UpdateNeedParameters parameters);
        
        Task<bool> DeleteNeedAsync(int needId);

        Task<bool> DeleteNeedAppliersAsync(int needId);

        Task<bool> ApplyNeedAsync(int needId, string applierCi);

        Task<bool> DeleteNeedApplierAsync(int needId, string applierCi);

        Task<bool> AcceptApplierAsync(ManageApplierParameters parameters);

        Task<bool> ChangeStatusAsync(int needId, string status);
    }
}
