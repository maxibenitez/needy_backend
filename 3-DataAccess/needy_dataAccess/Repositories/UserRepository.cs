using Dapper;
using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction.Parameters;

namespace needy_dataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Properties and Fields

        private readonly PostgreSQLConnection _dbConnection;

        #endregion

        #region Builders

        public UserRepository(PostgreSQLConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        #endregion

        #region Implments IUserRepository

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            using var connection = _dbConnection.CreateConnection();

            var query = @"
                            SELECT ""CI"", ""FirstName"", ""LastName"", ""Address"", ""City"", ""Zone"",
                                   ""Phone"", ""Gender"", ""BirthDate"", ""Email"", ""Password""
                            FROM public.""Users""";

            return await connection.QueryAsync<User>(query, new {});
        }

        public async Task<User> GetUserByCIAsync(int userCI)
        {
            using var connection = _dbConnection.CreateConnection();

            var query = @"
                            SELECT ""CI"", ""FirstName"", ""LastName"", ""Address"", ""City"", ""Zone"",
                                   ""Phone"", ""Gender"", ""BirthDate"", ""Email"", ""Password""
                            FROM public.""Users""
                            WHERE ""CI"" = @CI";

            return await connection.QueryFirstOrDefaultAsync<User>(query, new { CI = userCI });
        }

        public async Task<bool> InsertUserAsync(InsertUserParameters parameters)
        {
            using var connection = _dbConnection.CreateConnection();

            var query = @"
                            INSERT INTO public.""Users"" (""CI"", ""FirstName"", ""LastName"", ""Address"", ""City"", ""Zone"",
                                                        ""Phone"", ""Gender"", ""BirthDate"", ""Email"", ""Password"")
                            VALUES (@CI, @FirstName, @LastName, @Address, @City, @Zone, @Phone, @Gender, @BirthDate, @Email, @Password)";

            var result = await connection.ExecuteAsync(query, new
            {
                parameters.CI,
                parameters.FirstName,
                parameters.LastName,
                parameters.Address,
                parameters.Zone,
                parameters.Phone,
                parameters.Gender,
                parameters.BirthDate,
                parameters.Email,
                parameters.Password
            });

            return result > 0;
        }

        #endregion
    }
}
