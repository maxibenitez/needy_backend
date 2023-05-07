using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_dataAccess.Repositories
{
    public class NeedRepository : INeedRepository
    {
        #region Properties and Fields

        private readonly PostgreSQLConnection _dbConnection;

        #endregion

        #region Builders

        public NeedRepository(PostgreSQLConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        #endregion

        #region Implments INeedRepository

        public Task<IEnumerable<Need>> GetNeedsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Need>> GetNeedsBySkillAsync(string skill)
        {
            throw new NotImplementedException();
        }

        public Task<Need> GetNeedByIdAsync(int needId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertNeedAsync(InsertNeedParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateNeedAsync(int needId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteNeedAsync(int needId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApplyNeedAsync(int needId, int applierId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UnapplyNeedAsync(int needId, int applierId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AcceptApplierAsync(int applierId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeclineApplierAsync(int applierId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ChangeStatusAsync(string status)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
