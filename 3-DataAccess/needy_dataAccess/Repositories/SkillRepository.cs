using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace needy_dataAccess.Repositories
{
    public class SkillRepository: ISkillRepository
    {
        #region Properties and Fields

        private readonly PostgreSQLConnection _dbConnection;

        #endregion

        #region Builders
        
        public SkillRepository(PostgreSQLConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        #endregion

        #region Implements ISkillRepository

        public async Task<Skill> GetSkillByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Skill>> GetSkills()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
