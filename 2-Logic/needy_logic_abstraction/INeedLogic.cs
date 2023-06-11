using needy_dto;
using needy_logic_abstraction.Enumerables;
using needy_logic_abstraction.Parameters;

namespace needy_logic_abstraction
{
    public interface INeedLogic
    {
        Task<IEnumerable<Need>> GetNeedsAsync();

        Task<IEnumerable<Need>> GetNeedsBySkillAsync(int skillId);

        Task<Need> GetNeedByIdAsync(int needId);

        Task<IEnumerable<Need>> GetUserCreatedNeedsAsync(string userCI);

        Task<IEnumerable<Need>> GetUserAppliedNeedsAsync(string userCI);

        Task<bool> InsertNeedAsync(InsertNeedParameters parameters);

        Task<bool> UpdateNeedAsync(UpdateNeedParameters parameters);

        Task<bool> DeleteNeedAsync(int needId);

        Task<ErrorStatus> ApplyNeedAsync(int needId);

        Task<bool> UnapplyNeedAsync(int needId);

        Task<ErrorStatus> AcceptApplierAsync(ManageApplierParameters parameters);

        Task<bool> DeclineApplierAsync(ManageApplierParameters parameters);
    }
}
