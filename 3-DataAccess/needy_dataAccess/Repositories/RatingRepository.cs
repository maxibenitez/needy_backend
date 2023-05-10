using Dapper;
using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace needy_dataAccess.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        #region Properties and Fields

        private readonly PostgreSQLConnection _dbConnection;

        #endregion

        #region Builders

        public RatingRepository(PostgreSQLConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        #endregion

        #region Implments IRatingRepository

        public async Task<IEnumerable<Rating>> GetRatingByUserCiAsync(string userCi)
        {
            using var connection = _dbConnection.CreateConnection();

            var query = @"SELECT ""CiRquestor"", ""CiHelper"", ""RatingValue"", ""Comment"" 
                        FROM public.""Rating""
                        WHERE ""CiHelper"" = @CiHelper";
            return await connection.QueryAsync<Rating>(query, new { CiHelper = userCi});
        }

        public async Task<bool> InsertRatingAsync(InsertRatingParameters parameters)
        {
            using var connection = _dbConnection.CreateConnection();

            var query = @"
                            INSERT INTO public.""Rating"" (""CiRequestor"", ""CiHelper"", ""RatingValue"", ""Comment"")
                            VALUES (@CiRequestor, @CiHelper, @Rating, @Comment)";

            var result = await connection.ExecuteAsync(query, new
            {
                parameters.CiRequestor,
                parameters.CiHelper,
                parameters.Rating,
                parameters.Comment
            });

            return result > 0;
        }

        #endregion
    }
}
