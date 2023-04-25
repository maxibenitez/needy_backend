using Dapper;
using needy_dataAccess.Interfaces;
using needy_logic_abstraction.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace needy_dataAccess.Repositories
{
    public class RaitingRepository : IRaitingRepository
    {
        #region Properties and Fields

        private readonly PostgreSQLConnection _dbConnection;

        #endregion

        #region Builders

        public RaitingRepository(PostgreSQLConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        #endregion

        #region Implments IRaitingRepository

        public async Task<bool> InsertRaitingAsync(InsertRaitingParameters parameters)
        {
            using var connection = _dbConnection.CreateConnection();

            var query = @"
                            INSERT INTO public.""Raiting"" (""CiRequestor"", ""CiHelper"", ""Raiting"", ""Comment"")
                            VALUES (@CiRequestor, @CiHelper, @Raiting, @Comment)";

            var result = await connection.ExecuteAsync(query, new
            {
                parameters.CiRequestor,
                parameters.CiHelper,
                parameters.Raiting,
                parameters.Comment
            });

            return result > 0;
        }

        #endregion
    }
}
