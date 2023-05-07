using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_dataAccess.Interfaces
{
    public interface INeedRepository
    {
        Task<IEnumerable<Need>> GetNeedsAsync();

        Task<IEnumerable<Need>> GetNeedsBySkillAsync(string skill);

        Task<Need> GetNeedByIdAsync(int needId);

        Task<bool> InsertNeedAsync(InsertNeedParameters parameters);

        Task<bool> ApplyNeedAsync(int needId, int applierId);

        Task<bool> UnapplyNeedAsync(int needId, int applierId);

        Task<bool> UpdateNeedAsync(int needId);

        Task<bool> DeleteNeedAsync(int needId);

        Task<bool> AcceptApplierAsync(int applierId);

        Task<bool> DeclineApplierAsync(int applierId);

        Task<bool> ChangeStatusAsync(string status);
    }
}
