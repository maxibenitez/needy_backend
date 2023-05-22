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

        Task<string[]> GetNeedAppliersListAsync(int needId);

        Task<bool> InsertNeedAsync(InsertNeedParameters parameters);
        
        Task<bool> UpdateNeedAsync(int needId, UpdateNeedParameters parameters);
        
        Task<bool> DeleteNeedAsync(int needId);

        Task<bool> UpdateAppliersListAsync(int needId, string[] appliersCI);

        Task<bool> AcceptApplierAsync(int needId, string applierCI);

        Task<bool> ChangeStatusAsync(int needId, string status);
    }
}
