using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_logic_abstraction
{
    public interface INeedLogic
    {
        Task<IEnumerable<Need>> GetNeedsAsync();

        Task<IEnumerable<Need>> GetNeedsBySkillAsync(int skillId);

        Task<Need> GetNeedByIdAsync(int needId);

        Task<bool> InsertNeedAsync(InsertNeedParameters parameters);
        
        Task<bool> UpdateNeedAsync(int needId, UpdateNeedParameters parameters);
        
        Task<bool> DeleteNeedAsync(int needId);

        Task<bool> ApplyNeedAsync(int needId);

        Task<bool> UnapplyNeedAsync(int needId);

        Task<bool> AcceptApplierAsync(int needId, string applierCi);

        Task<bool> DeclineApplierAsync(int needId, string applierCi);
    }
}
