using needy_dto;
using needy_logic_abstraction.Parameters;
using Npgsql;
using System.Data;

namespace needy_dataAccess.Interfaces
{
    public interface INeedRepository
    {
        Task<IEnumerable<NeedData>> GetNeedsAsync();

        Task<IEnumerable<NeedData>> GetNeedsBySkillAsync(int skillId);

        Task<NeedData> GetNeedByIdAsync(int needId);

        Task<IEnumerable<string>> GetNeedAppliersAsync(int needId);

        Task<bool> InsertNeedAsync(string userCI, InsertNeedParameters parameters);
        
        Task<bool> UpdateNeedAsync(int needId, UpdateNeedParameters parameters);
        
        Task<bool> DeleteNeedAsync(int needId);

        Task<bool> DeleteNeedAppliersAsync(int needId);

        Task<bool> ApplyNeedAsync(int needId, string applierCi);

        Task<bool> DeleteNeedApplierAsync(int needId, string applierCi);

        Task<bool> AcceptApplierAsync(int needId, string applierCI);

        Task<bool> ChangeStatusAsync(int needId, string status);
    }
}
