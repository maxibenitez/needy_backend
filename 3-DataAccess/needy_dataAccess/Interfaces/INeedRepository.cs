using needy_dto;
using needy_logic_abstraction.Parameters;
using Npgsql;
using System.Data;

namespace needy_dataAccess.Interfaces
{
    public interface INeedRepository
    {
        Task<IDataReader> GetNeedsAsync();

        Task<IDataReader> GetNeedsBySkillAsync(int skillId);

        Task<NeedData> GetNeedByIdAsync(int needId);

        Task<bool> InsertNeedAsync(InsertNeedParameters parameters);
        
        Task<bool> UpdateNeedAsync(int needId, UpdateNeedParameters parameters);
        
        Task<bool> DeleteNeedAsync(int needId);

        Task<bool> ApplyNeedAsync(int needId, string applierCi);

        Task<bool> UnapplyNeedAsync(int needId, string applierCi);

        Task<bool> AcceptApplierAsync(int needId, string applierCi);

        Task<bool> DeclineApplierAsync(int needId, string applierCi);

        Task<bool> ChangeStatusAsync(int needId, string status);
    }
}
